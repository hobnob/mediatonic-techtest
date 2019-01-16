# Mediatonic Tech Test

## Available routes

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



## Running tests

All tests are built using NUnit 

