# Mediatonic Tech Test

## Contents
 - [Usage](#usage)
 - [Running Tests](#running-tests)
 - [Running the API](#running-the-api)
 - [Notes](#notes)
 - [Available Routes](#available-routes)

## Usage
The API is backed by a SQLite database to ensure that data is persistent even when the API isn't running. To begin, add a new user through the [POST User route](#post-v1users); add a new animal through the [POST Animal route](#post-v1animals); then assign the animal to the user using the [POST User/Animal route](#post-v1usersuseridanimals).

Refreshing the [GET User/Animal route](#get-v1usersuseridanimals) will show happiness decreasing and hunger increasing. These values range from -1 (Very Sad/Not Hungry at all) to 1 (Very Happy/Very Hungry). These values will continue to change even when the program is not running, or no one is connecting to the API.

Using the [Feed route](#put-v1usersuseridanimalsidfeed) and [Stroke route](#put-v1usersuseridanimalsidstroke) you can decrease hunger and increase happiness respectively.

## Running Tests
Tests are built using NUnit and can be run in Visual Studio 2017 by selecting `Test` -> `Run` -> `All Tests`. Some of these tests will take a little while to run, as they ensure things like descreasing of happiness over time.

## Running the API
The API is built using .Net Core and can be run in Visual Studio 2017 by selecting `Debug` -> `Start Debugging` or the Play button in the toolbar.

Once debugging as started you'll see the Swagger UI page. From there routes can either run in Swagger UI, or accessed by other means (such as directly in a browser, or through RESTED).

## Notes
There are a few missing features here - namely authentication, and the capability to update a user or animal. Other important notes on things that are missing/placeholder pieces:

 - The database is SQLite, which is obviously not scalable - this would in reality be something more appropriate like MariaDb or Postgres
 - HTTP is use rather than HTTPS - in production everything would be HTTPS
 - Happiness and Hunger would probably be better as a per-minute countdown in reality. Per-second works well in testing and when displaying countdowns over time, but minutes will work better in an actual game where the player might be doing actions over the course of hours

## Available Routes

### `GET /swagger`
A convienience route providing visual documentation and interacting with the API

### `GET /v1/Animals`
Gets all animals as a JSON array

#### Return Values
Code | Description
---------- | -------------
200 | Success

#### Example Success Response
```json
[
  {
    "id": 1,
    "typeName": "Dragon",
    "hungerPerSecond": 0.005,
    "sadnessPerSecond": 0.05
  },
  {
    "id": 2,
    "typeName": "Pixie",
    "hungerPerSecond": 0.001,
    "sadnessPerSecond": 0.00001
  }
]
```

### `POST /v1/Animals`
Creates a new animal with the specified properties

#### Payload Example
```json
{
  "typeName": "Fairy",
  "hungerPerSecond": 0.01,
  "sadnessPerSecond": 0.0001
}
```

#### Return Values
Code | Description
---------- | -------------
201 | Animal created successfully - location header provided
400 | An invalid parameter was entered - a string is returned

#### Example Success Response
```json
{
  "id": 3,
  "typeName": "Fairy",
  "hungerPerSecond": 0.01,
  "sadnessPerSecond": 0.0001
}
```

### `GET /v1/Animals/{id}`
Returns the animal with the specified ID (a positive integer)

#### Return Values
Code | Description
---------- | -------------
200 | Success
404 | The animal with that ID isn't found

#### Example Success Response
```json
{
  "id": 1,
  "typeName": "Dragon",
  "hungerPerSecond": 0.005,
  "sadnessPerSecond": 0.05
}
```

### `GET /v1/Users`
Gets all users as a JSON array

#### Return Values
Code | Description
---------- | -------------
200 | Success

#### Example Success Response
```json
[
  {
    "id": 1,
    "displayName": "batman"
  },
  {
    "id": 2,
    "displayName": "bwayne"
  }
]
```

### `POST /v1/Users`
Creates a new user with the specified properties

#### Payload Example
```json
{
  "displayName": "ckent"
}
```

#### Return Values
Code | Description
---------- | -------------
201 | User created successfully - location header provided
400 | An invalid parameter was entered - a string is returned

#### Example Success Response
```json
{
  "id": 3,
  "displayName": "ckent"
}
```

### `GET /v1/Users/{id}`
Returns the user with the specified ID (a positive integer)

#### Return Values
Code | Description
---------- | -------------
200 | Success
404 | The user with that ID isn't found

#### Example Success Response
```json
{
  "id": 1,
  "displayName": "batman"
}
```

### `GET /v1/Users/{userId}/Animals`
Gets all animals the specified user owns as a JSON array (`userId` is a positive integer)

#### Return Values
Code | Description
---------- | -------------
200 | Success
404 | The user specified wasn't found

#### Example Success Response
```json
[
  {
    "userId": 1,
    "animalId": 1,
    "happiness": -0.45,
    "hunger": 0.32
  },
  {
    "userId": 1,
    "animalId": 3,
    "happiness": 0.27,
    "hunger": -0.14
  }
]
```

### `POST /v1/Users/{userId}/Animals`
Sets an animal to be owned by a user (`userId` is positive integer) by passing a positive integer of the animal in the body

#### Payload Example
```json
2
```

#### Return Values
Code | Description
---------- | -------------
201 | User animal relation created successfully - location header provided
400 | An invalid parameter was entered - a string is returned
404 | User ID does not exist

#### Example Success Response
```json
{
  "userId": 1,
  "animalId": 2,
  "happiness": 0,
  "hunger": 0
}
```

### `GET /v1/Users/{userId}/Animals/{id}`

Returns the user with the specified ID (`userId` and `id` are positive integers)

#### Return Values
Code | Description
---------- | -------------
200 | Success
404 | The user or animal with that ID isn't found

#### Example Success Response
```json
{
  "userId": 1,
  "animalId": 1,
  "happiness": -0.65,
  "hunger": 0.73
}
```

### `PUT /v1/Users/{userId}/Animals/{id}/Feed`
Feeds the specified animal owned by the specified user (`userId` and `id` are positive integers)

#### Return Values
Code | Description
---------- | -------------
200 | Success
404 | The user or animal with that ID isn't found

#### Example Success Response
```json
{
  "userId": 1,
  "animalId": 1,
  "happiness": -0.65,
  "hunger": 0.23
}
```

### `PUT /v1/Users/{userId}/Animals/{id}/Stroke`
Strokes the specified animal owned by the specified user (`userId` and `id` are positive integers)

#### Return Values
Code | Description
---------- | -------------
200 | Success
404 | The user or animal with that ID isn't found

#### Example Success Response
```json
{
  "userId": 1,
  "animalId": 1,
  "happiness": -0.15,
  "hunger": 0.23
}
```
