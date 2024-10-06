# SlotAppointments
 Slot Appointments System

## Configuration 

You'll need to set the API base URL and the authentication credentials in your AppSettings.json file.

## Project Structure

Below is a breakdown of the project structure:

### `SlotAppointments.API/`
Contains API controllers that expose endpoints to clients. Controllers process HTTP requests, interact with services, and return responses.

### `Services/`
Contains the core service layer logic. Services are responsible for orchestrating domain logic and interacting with external systems through service agents.

### `ServiceAgents/`
Defines the service agents responsible for communicating with external APIs.

### `Domain/`
Contains the core domain entities and business logic.

- **Entities**: Represents the main objects used in the business domain.
  - `Availability`: Represents the availability of a facility, handling working hours, lunch breaks, and busy slots.
  - `Slot`: Represents a single slot in the facility, with start and end times.
  - `Facility`: Represents the service facility itself, where slots are managed.

### `Tests/`
Contains unit and integration tests.
