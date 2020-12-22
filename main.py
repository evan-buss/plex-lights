import asyncio
import json
from pprint import pprint
from os.path import exists

import aiofiles
from fastapi import FastAPI
from pydantic.json import pydantic_encoder
from pywizlight import wizlight
from starlette.requests import Request

from models import Pairing, Data

app = FastAPI()


def load_data() -> Data:
    if not exists("config.json"):
        return Data()

    with open("config.json") as f:
        data = json.load(f)
        pprint(data)
        return Data(history=data["history"], pairings=data["pairings"])


data = load_data()


@app.get("/")
async def read_root(request: Request):
    json = await request.json()
    print(json)

    return {"Hello": "World"}


@app.post("/hook")
async def web_hook(request: Request):
    form = await request.form()
    payload = json.loads(form["payload"])
    pprint(payload["Account"])
    pprint(payload["Server"])
    pprint(payload["Player"])

    client_id = payload["Player"]["uuid"]

    data.history.append(client_id)
    data.history = data.history[:5]
    await persist_data()

    pprint(data.pairings)

    pairing = next((x for x in data.pairings if x["client_id"] == client_id), None)

    if pairing:
        bulbs = [wizlight(x) for x in pairing["bulb_ips"]]

        tasks = []
        for bulb in bulbs:
            if payload["event"] == "media.play" or payload["event"] == "media.resume":
                tasks.append(asyncio.create_task(bulb.turn_off()))
            elif payload["event"] == "media.pause" or payload["event"] == "media.stop":
                tasks.append(asyncio.create_task(bulb.turn_on()))

        await asyncio.gather(*tasks)


@app.get("/history")
def get_history():
    return data.history


async def persist_data():
    async with aiofiles.open("config.json", "w") as f:
        await f.write(json.dumps(data, indent=4, default=pydantic_encoder))


# Submit pairing of client id and bulb ip
# Debug endpoint that returns the client ids of the last 5
