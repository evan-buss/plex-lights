# Plex Webhooks Integration for Wiz Lights

## Usage

1. Run server via docker or manually.
2. Add webhook to Plex settings pointing to the ip:port where the server is running.
3. Play something on a plex device.
4. Open the website, you should see the device listed.
5. Press "Refresh" to discover Wiz lights on the network or enter the IP manually.
6. Enter client id and IP in the form.
   - Click any of list item to autofill the form inputs.
7. Click "Save" on the form. Your configuration will appear.
   - Each client id can have multiple light bulbs associated with it.
8. Play something on plex and the light(s) will go off. Pause it/they will come back on.

## Issues

- Bulb discovery doesn't work in Docker container. Tryin to figure out the cause.
