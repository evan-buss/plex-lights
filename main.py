import asyncio
import json

from fastapi import FastAPI, Form
from fastapi.params import Depends
from fastapi.staticfiles import StaticFiles
from fastapi.templating import Jinja2Templates
from pywizlight import wizlight
from sqlalchemy.orm.session import Session
from starlette.requests import Request
from starlette.responses import HTMLResponse, RedirectResponse

from database import crud
from database.database import Base, SessionLocal, engine

app = FastAPI()


app.mount("/static", StaticFiles(directory="static"), name="static")

templates = Jinja2Templates(directory="templates")


Base.metadata.create_all(bind=engine)


# Dependency
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()


@app.get("/", response_class=HTMLResponse)
async def management_page(request: Request, db: Session = Depends(get_db)):
    return templates.TemplateResponse(
        "index.jinja",
        {
            "request": request,
            "devices": crud.get_devices(db),
            "configs": crud.get_configs(db),
            "lights": crud.get_lights(db),
        },
    )


@app.post("/add_config", response_class=RedirectResponse)
async def add_config(
    db: Session = Depends(get_db),
    name: str = Form(...),
    light_id: int = Form(...),
    device_id: int = Form(...),
):
    crud.create_config(db, name, light_id, device_id)
    return RedirectResponse("/", status_code=302)


@app.delete("/delete/{config_id}")
async def delete_config(config_id: int, db: Session = Depends(get_db)):
    crud.delete_config(db, config_id)


@app.post("/add_light", response_class=RedirectResponse)
async def add_light(
    db: Session = Depends(get_db), name: str = Form(...), ip: str = Form(...)
):
    crud.create_light(db, name, ip)
    return RedirectResponse("/", status_code=302)


@app.post("/hook")
async def plex_hook(request: Request, db: Session = Depends(get_db)):
    form = await request.form()
    payload = json.loads(form["payload"])
    client_name = payload["Player"]["title"]
    client_id = payload["Player"]["uuid"]

    crud.upsert_device(db, client_name, client_id)

    config = crud.get_config_by_device(db, client_id)

    event = payload["event"]
    print(event)
    bulbs = [wizlight(x.ip) for x in config.lights]
    tasks = []
    for bulb in bulbs:
        if event == "media.play" or event == "media.resume":
            tasks.append(
                asyncio.create_task(
                    bulb.turn_on(PilotBuilder(warm_white=255, brightness=0))
                )
            )
        elif event == "media.pause" or event == "media.stop":
            tasks.append(
                asyncio.create_task(
                    bulb.turn_on(PilotBuilder(warm_white=255, brightness=255))
                )
            )

    await asyncio.gather(*tasks)
