import asyncio
import json
from typing import List

from fastapi import FastAPI, Form
from fastapi.templating import Jinja2Templates
from pywizlight import wizlight
from pywizlight.discovery import find_wizlights
from starlette.requests import Request
from starlette.responses import HTMLResponse, RedirectResponse

from util import *

app = FastAPI()

templates = Jinja2Templates(directory="templates")


data = load_data()


@app.middleware("http")
async def persist_data_middleware(request: Request, call_next):
    response = await call_next(request)
    persist_data(data)
    return response


@app.get("/", response_class=HTMLResponse)
async def management_page(request: Request):
    return templates.TemplateResponse(
        "plex.html",
        {
            "request": request,
            "history": data.history,
            "clients": data.clients,
            "available_bulbs": data.available_bulbs,
        },
    )


@app.get("/discover", response_model=List[str])
async def discover_lights():
    bulbs = await find_wizlights(broadcast_address="192.168.1.255")
    data.available_bulbs = [bulb.ip_address for bulb in bulbs]
    return data.available_bulbs


@app.post("/add", response_class=RedirectResponse)
async def add_config(client_id: str = Form(...), bulb_ip: str = Form(...)):
    client_id = client_id.strip()
    bulb_ips = data.clients.get(client_id, set())
    bulb_ips.add(bulb_ip)
    data.clients[client_id] = bulb_ips
    return RedirectResponse("/", status_code=302)


@app.delete("/delete/{client_id}")
async def delete_config(client_id: str):
    try:
        del data.clients[client_id]
    except:
        pass


@app.post("/hook")
async def plex_hook(request: Request):
    form = await request.form()
    payload = json.loads(form["payload"])
    client_name = payload["Player"]["title"]
    client_id = payload["Player"]["uuid"]

    data.history.add((client_name, client_id))

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
