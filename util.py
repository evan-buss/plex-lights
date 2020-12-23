import pickle
from models import Data
import os.path as path


def load_data() -> Data:
    if not path.exists("config.pickle"):
        return Data()

    with open("config.pickle", "rb") as f:
        return pickle.load(f)


def persist_data(data: Data) -> None:
    with open("config.pickle", "wb") as f:
        pickle.dump(data, f)
