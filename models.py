from dataclasses import dataclass
import dataclasses
from typing import List


@dataclass
class Pairing:
    client_id: str
    bulb_ips: List[str] = dataclasses.field(default_factory=lambda: [])


@dataclass
class Data:
    history: List[str] = dataclasses.field(default_factory=lambda: [])
    pairings: List[Pairing] = dataclasses.field(default_factory=lambda: [])
