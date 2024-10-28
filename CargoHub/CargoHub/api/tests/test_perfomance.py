import requests
import concurrent.futures
import time

# Define the URL for your API endpoint
API_URL = "http://localhost:3000/api/v1/locations/4"

# Define the number of concurrent requests you want to test
CONCURRENT_REQUESTS = 100

# Function to send a request to the API and measure response time
def send_request():
    headers = {
        "api_key": "a1b2c3d4e5"
    }
    try:
        start_time = time.time()
        response = requests.get(API_URL, headers=headers)
        response_time = time.time() - start_time
        return response.status_code, response_time
    except requests.exceptions.RequestException as e:
        return None, e  # Capture any request errors

# Main test function to run concurrent requests
def test_concurrent_requests():
    success_count = 0
    failure_count = 0
    total_response_time = 0

    with concurrent.futures.ThreadPoolExecutor(max_workers=CONCURRENT_REQUESTS) as executor:
        # Start a batch of requests and collect results
        futures = [executor.submit(send_request) for _ in range(CONCURRENT_REQUESTS)]
        
        for future in concurrent.futures.as_completed(futures):
            status, result = future.result()
            
            if status == 200:
                success_count += 1
                total_response_time += result  # Add the response time for successful requests
            else:
                failure_count += 1
                print(f"Request failed: {result}")

    # Calculate and print summary results
    avg_response_time = total_response_time / success_count if success_count else float('inf')
    
    print(f"Total Requests: {CONCURRENT_REQUESTS}")
    print(f"Successful Requests: {success_count}")
    print(f"Failed Requests: {failure_count}")
    print(f"Average Response Time: {avg_response_time:.2f} seconds")

# Run the test
if __name__ == "__main__":
    test_concurrent_requests()
