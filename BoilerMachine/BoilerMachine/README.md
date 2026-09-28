## Description of the Project

# Initialization
* As application starts, display a welcoming message
* Initialize the boiler's system status to "Lockout"
* The first log entry should be "Boiler initialized" with 
timestamp

# Console Interface Presentation
* The requirements for the interface was
  * Start
  * Stop
  * Simulate Boiler error
  * Toggle switch
  * Reset lockout
  * View event log
  * Exit

# Run Interlock Simulation
* At initialization, the switch is "Open"
* The user is able to toggle the switch
* The boiler can be ready or started if switch is in 
"Closed" position
* Each time the switch state changes, log the event with a
message like "Interlock Switch toggled to some state"

# Lockout Reset Functionality
* Reset is essential after any failure or after initialization
* If "Run Interlock" switch is in "Closed" position, the system
status should transition to "Ready"
* Log the event "Boiler status changed to ready."

# Boiler Start Sequence
* Pre purge cycle - 10 seconds - log to file
* Ignition - 10 seconds - log to file
* Operational after the ignition phase is done.

# Event Logging Mechanism
* Logged data into files

# Error Handling
* Display clear error message : "Error: [Error Description]. System in Lockout"
