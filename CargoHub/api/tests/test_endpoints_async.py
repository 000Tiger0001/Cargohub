import aiohttp
import asyncio
import pytest
import time

# pip install pytest-asyncio

# Define the URLs for your API endpoints
API_URLS = [
    "http://localhost:3000/api/v1/clients",
    "http://localhost:3000/api/v1/clients/1",
    "http://localhost:3000/api/v1/clients/1/orders",

    "http://localhost:3000/api/v1/inventories",
    "http://localhost:3000/api/v1/inventories/1",

    "http://localhost:3000/api/v1/item_groups",
    "http://localhost:3000/api/v1/item_groups/1",
    "http://localhost:3000/api/v1/item_groups/1/items",

    "http://localhost:3000/api/v1/item_lines",
    "http://localhost:3000/api/v1/item_lines/1",
    "http://localhost:3000/api/v1/item_lines/1/items",

    "http://localhost:3000/api/v1/item_types",
    "http://localhost:3000/api/v1/item_types/1",
    "http://localhost:3000/api/v1/item_types/1/items",

    "http://localhost:3000/api/v1/items",
    "http://localhost:3000/api/v1/items/P000001",
    "http://localhost:3000/api/v1/items/P000001/inventory",
    "http://localhost:3000/api/v1/items/P000001/inventory/totals",

    "http://localhost:3000/api/v1/locations",
    "http://localhost:3000/api/v1/locations/1",

    "http://localhost:3000/api/v1/orders",
    "http://localhost:3000/api/v1/orders/1",
    "http://localhost:3000/api/v1/orders/1/items",

    "http://localhost:3000/api/v1/shipments",
    "http://localhost:3000/api/v1/shipments/1",
    "http://localhost:3000/api/v1/shipments/1/items",

    "http://localhost:3000/api/v1/suppliers",
    "http://localhost:3000/api/v1/suppliers/1",
    "http://localhost:3000/api/v1/suppliers/1/items",

    "http://localhost:3000/api/v1/transfers",
    "http://localhost:3000/api/v1/transfers/1",
    "http://localhost:3000/api/v1/transfers/1/items",

    "http://localhost:3000/api/v1/warehouses",
    "http://localhost:3000/api/v1/warehouses/1",
    "http://localhost:3000/api/v1/warehouses/1/locations"
]

CONCURRENT_REQUESTS = 20

async def send_request(session, api_url):
    headers = {"api_key": "a1b2c3d4e5"}
    try:
        start_time = time.time()
        async with session.get(api_url, headers=headers) as response:
            response_time = time.time() - start_time
            return api_url, response.status, response_time
    except aiohttp.ClientError as e:
        return api_url, None, e

@pytest.mark.asyncio
async def test_concurrent_requests():
    results = {url: {"success": 0, "failure": 0, "total_response_time": 0} for url in API_URLS}

    async with aiohttp.ClientSession() as session:
        tasks = [
            send_request(session, url)
            for url in API_URLS
            for _ in range(CONCURRENT_REQUESTS)
        ]
        
        for future in asyncio.as_completed(tasks):
            api_url, status, result = await future
            if status == 200:
                results[api_url]["success"] += 1
                results[api_url]["total_response_time"] += result
            else:
                results[api_url]["failure"] += 1
                print(f"Request to {api_url} failed: {result}")

    # Print summary results for each endpoint
    for api_url, stats in results.items():
        avg_response_time = (
            stats["total_response_time"] / stats["success"]
            if stats["success"] > 0
            else float('inf')
        )
        
        print(f"\nEndpoint: {api_url}")
        print(f"Total Requests: {CONCURRENT_REQUESTS}")
        print(f"Successful Requests: {stats['success']}")
        print(f"Failed Requests: {stats['failure']}")
        print(f"Average Response Time: {avg_response_time:.2f} seconds")
