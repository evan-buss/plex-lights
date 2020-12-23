from dataclasses import dataclass
import dataclasses
from typing import Dict, List, Set, Tuple


@dataclass
class Data:
    available_bulbs: Set[str] = dataclasses.field(default_factory=lambda: [])
    history: Set[Tuple[str, str]] = dataclasses.field(default_factory=lambda: set())
    clients: Dict[str, Set[str]] = dataclasses.field(default_factory=lambda: {})
