import json

from models.base import Base

ITEM_LINES = []


class ItemLines(Base):
    def __init__(self, root_path, is_debug=False) -> None:
        self.data_path = root_path + "item_lines.json"
        self.load(is_debug)

    def get_item_lines(self) -> list:
        return self.data

    def get_item_line(self, item_line_id) -> dict or None:  # type: ignore
        for x in self.data:
            if x["id"] == item_line_id:
                return x
        return None

    def add_item_line(self, item_line) -> None:
        item_line["created_at"] = self.get_timestamp()
        item_line["updated_at"] = self.get_timestamp()
        self.data.append(item_line)

    def update_item_line(self, item_line_id, item_line) -> None:
        item_line["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == item_line_id:
                self.data[i] = item_line
                break

    def remove_item_line(self, item_line_id) -> None:
        # removes a item_line
        for x in self.data:
            if x["id"] == item_line_id:
                self.data.remove(x)

    def load(self, is_debug) -> None:
        # sets self.data to ITEM_LINES if debug is true or loads from file if debug is false
        if is_debug:
            self.data = ITEM_LINES
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self) -> None:
        # saves self.data to file
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()
