import requests
import concurrent.futures
import time

# to run test: pytest test_perfomance.py -s

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

# Define the number of concurrent requests you want to test per endpoint
CONCURRENT_REQUESTS = 20

# Function to send a request to a specific API endpoint and measure response time
def send_request(api_url):
    headers = {
        "api_key": "a1b2c3d4e5"
    }
    try:
        start_time = time.time()
        response = requests.get(api_url, headers=headers)
        response_time = time.time() - start_time
        return api_url, response.status_code, response_time
    except requests.exceptions.RequestException as e:
        return api_url, None, e  # Capture any request errors

# Main test function to run concurrent requests across multiple endpoints
def test_concurrent_requests():
    results = {url: {"success": 0, "failure": 0, "total_response_time": 0} for url in API_URLS}

    with concurrent.futures.ThreadPoolExecutor(max_workers=CONCURRENT_REQUESTS) as executor:
        # Start a batch of requests for each endpoint and collect results
        futures = [executor.submit(send_request, url) for url in API_URLS for _ in range(CONCURRENT_REQUESTS)]
        
        for future in concurrent.futures.as_completed(futures):
            api_url, status, result = future.result()
            
            if status == 200:
                results[api_url]["success"] += 1
                results[api_url]["total_response_time"] += result  # Add the response time for successful requests
            else:
                results[api_url]["failure"] += 1
                print(f"Request to {api_url} failed: {result}")

    # Calculate and print summary results for each endpoint
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

# Run the test
if __name__ == "__main__":
    test_concurrent_requests()
