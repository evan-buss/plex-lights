from typing import List
from sqlalchemy import Boolean, Column, ForeignKey, Integer, String
from sqlalchemy.orm import relationship
from sqlalchemy.sql.schema import Table

from .database import Base

association_table = Table(
    "device_lights",
    Base.metadata,
    Column("config_id", Integer, ForeignKey("configs.config_id")),
    Column("light_id", Integer, ForeignKey("lights.light_id")),
)


# Plex Device
class Device(Base):
    __tablename__ = "devices"

    device_id = Column(Integer, primary_key=True, index=True)
    name = Column(String)
    client_id = Column(String, index=True, unique=True)


# Wiz Light Bulb
class Light(Base):
    __tablename__ = "lights"

    light_id = Column(Integer, primary_key=True, index=True)
    name = Column(String)
    ip = Column(String, index=True, unique=True)


class Config(Base):
    __tablename__ = "configs"

    config_id = Column(Integer, primary_key=True, index=True)
    name = Column(String)
    is_active = Column(Boolean, default=True)
    lights: List[Light] = relationship("Light", secondary=association_table)

    device_id = Column(Integer, ForeignKey(Device.device_id))
    device = relationship("Device", foreign_keys="Config.device_id")
