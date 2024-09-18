import json

from models.base import Base

TRANSFERS = []


class Transfers(Base):
    def __init__(self, root_path, is_debug=False) -> None:
        self.data_path = root_path + "transfers.json"
        self.load(is_debug)

    def get_transfers(self) -> list:
        # returns all transfers available
        return self.data

    def get_transfer(self, transfer_id) -> dict or None:  # type: ignore
        # returns a transfer by id or None if not found
        for x in self.data:
            if x["id"] == transfer_id:
                return x
        return None

    def get_items_in_transfer(self, transfer_id) -> list or None:  # type: ignore
        # returns items in a transfer by id or None if not found
        for x in self.data:
            if x["id"] == transfer_id:
                return x["items"]
        return None

    def add_transfer(self, transfer) -> None:
        # adds a new transfer
        transfer["transfer_status"] = "Scheduled"
        transfer["created_at"] = self.get_timestamp()
        transfer["updated_at"] = self.get_timestamp()
        self.data.append(transfer)

    def update_transfer(self, transfer_id, transfer) -> None:
        # updates a transfer
        transfer["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == transfer_id:
                self.data[i] = transfer
                break

    def remove_transfer(self, transfer_id) -> None:
        # removes a transfer
        for x in self.data:
            if x["id"] == transfer_id:
                self.data.remove(x)

    def load(self, is_debug) -> None:
        # sets self.data to TRANSFERS if debug is true or loads from file if debug is false
        if is_debug:
            self.data = TRANSFERS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self) -> None:
        # saves self.data to file
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()
