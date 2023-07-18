#!/bin/bash

sleep 2 # Sleep for 2s to give the server enough time to start

python3 /app/wwwroot/Scripts/client.py --agent_id 1 --server_address=20.113.31.254:8787


# This will allow you to use CTRL+C to stop all background processes
trap "trap - SIGTERM && kill -- -$$" SIGINT SIGTERM
# Wait for all background processes to complete
wait