GET /api/users: Get all users
GET /api/users/{id}: Get a specific user
POST /api/users: Create a new user
PUT /api/users/{id}: Update an existing user
DELETE /api/users/{id}: Delete a user

## Examples

user- post request example:
{

<!-- id is not mandatory for neither -->

"id": 5,

<!-- username and email are mandatory for post requests -->

"username": "moloko",
"email": "moloko@email.com",

<!-- notmandatory but needed for update requests -->

"createdEvents": null,  
"attendingEvents": null,
"reviews": null,
"comments": null
}

API Endpoints and JSON Examples
Users
POST /api/Users
Create a new user
Request body:
jsonCopy{
"username": "johndoe",
"email": "john@example.com"
}
Response:
jsonCopy{
"id": 1,
"username": "johndoe",
"email": "john@example.com"
}
Events
GET /api/Events
Get all events
Response:
jsonCopy[
{
"id": 1,
"title": "Summer Concert",
"description": "Annual summer concert in the park",
"startTime": "2024-07-15T18:00:00Z",
"endTime": "2024-07-15T22:00:00Z",
"location": "Central Park",
"creator": {
"id": 1,
"username": "johndoe",
"email": "john@example.com"
}
}
]
GET /api/Events/{id}
Get a specific event
Response:
jsonCopy{
"id": 1,
"title": "Summer Concert",
"description": "Annual summer concert in the park",
"startTime": "2024-07-15T18:00:00Z",
"endTime": "2024-07-15T22:00:00Z",
"location": "Central Park",
"creator": {
"id": 1,
"username": "johndoe",
"email": "john@example.com"
},
"attendees": [
{
"id": 2,
"username": "janedoe",
"email": "jane@example.com"
}
],
"reviews": [
{
"id": 1,
"content": "Great event!",
"rating": 5,
"userId": 2
}
],
"comments": [
{
"id": 1,
"content": "Looking forward to it!",
"userId": 2
}
]
}
POST /api/Events
Create a new event
Request body:
jsonCopy{
"title": "Tech Meetup",
"description": "Monthly tech meetup for developers",
"startTime": "2024-10-01T19:00:00Z",
"endTime": "2024-10-01T21:00:00Z",
"location": "Tech Hub",
"creatorId": 1
}
Response:
jsonCopy{
"id": 2,
"title": "Tech Meetup",
"description": "Monthly tech meetup for developers",
"startTime": "2024-10-01T19:00:00Z",
"endTime": "2024-10-01T21:00:00Z",
"location": "Tech Hub",
"creatorId": 1
}
PUT /api/Events/{id}
Update an event
Request body:
jsonCopy{
"title": "Updated Tech Meetup",
"description": "Updated description for monthly tech meetup",
"startTime": "2024-10-01T18:30:00Z",
"endTime": "2024-10-01T21:30:00Z",
"location": "New Tech Hub"
}
DELETE /api/Events/{id}
Delete an event
POST /api/Events/{id}/attend
Attend an event
Request body:
jsonCopy{
"userId": 2
}
DELETE /api/Events/{id}/attend
Unattend an event
Request body:
jsonCopy{
"userId": 2
}
Comments
GET /api/Comments
Get all comments
Response:
jsonCopy[
{
"id": 1,
"content": "Looking forward to it!",
"user": {
"id": 2,
"username": "janedoe",
"email": "jane@example.com"
},
"event": {
"id": 1,
"title": "Summer Concert"
}
}
]
GET /api/Comments/{id}
Get a specific comment
Response:
jsonCopy{
"id": 1,
"content": "Looking forward to it!",
"user": {
"id": 2,
"username": "janedoe",
"email": "jane@example.com"
},
"event": {
"id": 1,
"title": "Summer Concert"
}
}
POST /api/Comments
Create a new comment
Request body:
jsonCopy{
"content": "Can't wait for this event!",
"userId": 2,
"eventId": 1
}
Response:
jsonCopy{
"id": 2,
"content": "Can't wait for this event!",
"userId": 2,
"eventId": 1
}
PUT /api/Comments/{id}
Update a comment
Request body:
jsonCopy{
"content": "Updated comment: Really excited for this event!"
}
DELETE /api/Comments/{id}
Delete a comment
Reviews (Assuming similar structure to Comments)
GET /api/Reviews
Get all reviews
GET /api/Reviews/{id}
Get a specific review
POST /api/Reviews
Create a new review
Request body:
jsonCopy{
"content": "Amazing event, highly recommended!",
"rating": 5,
"userId": 2,
"eventId": 1
}
PUT /api/Reviews/{id}
Update a review
DELETE /api/Reviews/{id}
Delete a review
