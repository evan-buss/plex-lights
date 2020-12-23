import asyncio
import json

from fastapi import FastAPI, Form
from fastapi.templating import Jinja2Templates
from pywizlight import wizlight
from starlette.requests import Request
from starlette.responses import HTMLResponse, RedirectResponse

from util import *

app = FastAPI()

templates = Jinja2Templates(directory="templates")


data = load_data()


@app.get("/", response_class=HTMLResponse)
async def management_page(request: Request):
    return templates.TemplateResponse(
        "plex.html",
        {"request": request, "history": data.history, "clients": data.clients},
    )


@app.post("/add", response_class=RedirectResponse)
async def add_config(client_id: str = Form(...), bulb_ip: str = Form(...)):
    client_id = client_id.strip()
    bulb_ips = data.clients.get(client_id, set())
    bulb_ips.add(bulb_ip)
    data.clients[client_id] = bulb_ips
    persist_data(data)
    return RedirectResponse("/", status_code=302)


@app.delete("/delete/{client_id}")
async def delete_config(client_id: str):
    print(client_id)
    try:
        del data.clients[client_id]
        persist_data(data)
    except:
        pass


@app.post("/hook")
async def plex_hook(request: Request):
    form = await request.form()
    payload = json.loads(form["payload"])
    client_name = payload["Player"]["title"]
    client_id = payload["Player"]["uuid"]

    data.history.add((client_name, client_id))
    persist_data(data)

    bulb_ips = data.clients.get(client_id, None)
    if bulb_ips:
        event = payload["event"]
        bulbs = [wizlight(x) for x in bulb_ips]
        tasks = []
        for bulb in bulbs:
            if event == "media.play" or event == "media.resume":
                tasks.append(asyncio.create_task(bulb.turn_off()))
            elif event == "media.pause" or event == "media.stop":
                tasks.append(asyncio.create_task(bulb.turn_on()))

        await asyncio.gather(*tasks)


@app.get("/history")
def get_history():
    return data.history
