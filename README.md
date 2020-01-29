# TrueLayer_NitinJain

This api is used to connect to TrueLayer Api to fetch Mock Bank Account details
API has 3 controller which exposing the method explained later
Controller are:
1. AccountsController : gives account and account transactions details
2. AuthController : Used to manage user authorization flow
3. CallbackController : handling the callback from the TrueLayer auth dialog (callback uri: http://localhost:3000/callback)

Controllers interact with the api service to get data from truelayer.

# build the application and launch in debug mode

AccountsController Method available are:

1. Get Accounts details
URI: http://localhost:3000/accounts/56c7b029e0f8ec5a2334fb0ffc2fface

2. Get Account details by account id
URI: http://localhost:3000/accounts/56c7b029e0f8ec5a2334fb0ffc2fface

3. Get Transactions details for an account id
URI: http://localhost:3000/accounts/56c7b029e0f8ec5a2334fb0ffc2fface/transactions

4. Get Min Max Amount and other Transactions details for an account id .. showing as a snapshot
URI: http://localhost:3000/accounts/56c7b029e0f8ec5a2334fb0ffc2fface/transactions/snapshot

# appsettings.Development.json
This json capture all the API level details. Used HandlerSettings class to map the appsettings

# Security
TrueLayer client_Id & client_secret are encoded to base64 in appsettings and will be decoded before sending to TrueLayer
secret.json is used to save the appsettings on development machine

Client_Id & Client_Secret can be saved at database level in encrypted (using passwordhash+salt) form and when API requires the credentials it will decrypted and then will be passed to TrueLayer. This way API creds in encrypted form will not be misused by client if got the access. Alsom the database table which store api secret can be secured based on table grant access. 

