import threading

NOTIFICATION_UPDATE_INTERVAL_SEC = 30

_queue = ["Dummy message"]


def push(notification) -> None:
    # Add notification to the queue
    global _queue
    _queue.append(notification)


def send() -> None:
    if len(_queue) > 0:
        # deletes the first notification from the queue and saves it in a variable
        notification = _queue.pop(0)
        # prints the notification
        print(notification)
    threading.Timer(NOTIFICATION_UPDATE_INTERVAL_SEC, send).start()


def start() -> None:
    send()
