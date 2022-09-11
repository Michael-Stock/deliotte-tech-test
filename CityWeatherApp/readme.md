# Deliotte Technical Test

## The task 📈

- Add City - adds city name, state (i.e. geographic sub-region), country, tourist rating
  (1-5), date established and estimated population. Adds record to local SQL data
  store and generates unique city id.
- Update city – update rating, date established and estimated population by city id
- Delete city – delete city by city id
- Search city – search by city name, and returns the city id, name, state (i.e.
  geographic sub-region), country, tourist rating (1-5), date established, estimated
  population, 2 digit country code, 3 digit country code, currency code and weather
  for the city. If there are multiple matches, this information is returned for all
  matches. If the city is not stored locally no results need be returned. The APIs above
  should be used to provide any information not stored locally.

## How it works

This application uses sqlite as a database store that will store a file on the local server when the application is run

## TODO

- Add code coverage
- Logging
- Better exception handling
- API security
- Tests using BDD at the API level, so they could be written by QA if required
- CI/CD setup
- Swagger docs
- API validation of the input

Many more cases need to be handled, some include:

- Countries returned as a list, need to ensure we match the right one (Currently picks the first)
- Weather is returned as a list, need to send more information to get the right one if there are duplicates
- Better error handling around the third party APIs if they don't return data or throw exceptions
- Partial name matching
  - New -> Newport, New York, currently it does an exact string match
- The tests document all the currently supported scenarios
