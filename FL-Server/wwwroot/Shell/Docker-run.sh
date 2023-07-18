#!/bin/bash

sleep 2 # Sleep for 2s to give the server enough time to start

Docker run -it -p 80:8787 flclients-arm 

# This will allow you to use CTRL+C to stop all background processes
trap "trap - SIGTERM && kill -- -$$" SIGINT SIGTERM
# Wait for all background processes to complete
wait