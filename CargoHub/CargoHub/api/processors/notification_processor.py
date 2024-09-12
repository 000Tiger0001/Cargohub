import threading

NOTIFICATION_UPDATE_INTERVAL_SEC = 30

_queue = ["Dummy message"]


def push(notification) -> None:
    global _queue
    _queue.append(notification)


def send() -> None:
    if len(_queue) > 0:
        notification = _queue.pop(0)
        print(notification)
    threading.Timer(NOTIFICATION_UPDATE_INTERVAL_SEC, send).start()


def start() -> None:
    send()
