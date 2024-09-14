import json

from models.base import Base

CLIENTS = []


class Clients(Base):
    def __init__(self, root_path, is_debug=False) -> None:
        self.data_path = root_path + "clients.json"
        self.load(is_debug)

    def get_clients(self) -> list:
        return self.data

    def get_client(self, client_id) -> dict or None:  # type: ignore
        for x in self.data:
            if x["id"] == client_id:
                return x
        return None

    def add_client(self, client) -> None:
        client["created_at"] = self.get_timestamp()
        client["updated_at"] = self.get_timestamp()
        self.data.append(client)

    def update_client(self, client_id, client) -> None:
        client["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == client_id:
                self.data[i] = client
                break

    def remove_client(self, client_id) -> None:
        # removes a client
        for x in self.data:
            if x["id"] == client_id:
                self.data.remove(x)

    def load(self, is_debug) -> None:
        # sets self.data to CLIENTS if debug is true or loads from file if debug is false
        if is_debug:
            self.data = CLIENTS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self) -> None:
        # saves self.data to file
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()
