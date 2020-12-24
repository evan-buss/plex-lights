from typing import List, Union

from sqlalchemy.orm.session import Session

from .models import Config, Device, Light


def get_devices(db: Session) -> List[Device]:
    return db.query(Device).all()


def upsert_device(db: Session, client_name: str, client_id: str) -> None:
    device = db.query(Device).filter(Device.client_id == client_id).scalar()

    if device is None:
        device = Device(name=client_name, client_id=client_id)
        db.add(device)
        db.commit()


def create_light(db: Session, name: str, ip: str) -> None:
    light = Light(name=name, ip=ip)
    db.add(light)
    db.commit()


def get_lights(db: Session) -> List[Light]:
    return db.query(Light).all()


def get_configs(db: Session) -> List[Device]:
    return db.query(Config).all()


def create_config(db: Session, name: str, light_id: int, device_id: int) -> None:
    light = db.query(Light).filter(Light.light_id == light_id).scalar()
    config = db.query(Config).filter(Config.device_id == device_id).scalar()

    if config:
        config.lights.append(light)
    else:
        config = Config(name=name, device_id=device_id, lights=[light])
        db.add(config)
    db.commit()


def delete_config(db: Session, config_id: int) -> None:
    config = db.query(Config).filter(Config.config_id == config_id).scalar()
    db.delete(config)
    db.commit()


def get_config_by_device(db: Session, client_id: str) -> Union[Config, None]:
    return db.query(Config).join(Device).filter(Device.client_id == client_id).first()
