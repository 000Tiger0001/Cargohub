from datetime import datetime


class Base:
    def __init__() -> None:
        pass

    def get_timestamp(self) -> str:
        return datetime.utcnow().isoformat() + "Z"
