# Kealan-Unhurd

This is Kealan O'Callaghan's project for the Un:Hurd position Tech Assessment.

## Running the project

- Download the project and open Kealan-Unhurd in Visual Studio 2022

- When the project is running, you must first run the **GetArtistById** function by pasting the following URL into your browser

```
http://localhost:7055/api/GetArtistById/{SpotifyID}
```

### Request

- Replace the SpotifyID with an ID of any Spotify artist, this can be found by clicking the ... three dots on the artists page. Below is some examples.
    * Fixate (Drum and Bass) - 1nB5SyBxZpy6ZhBOhjOkhw
    * Rene Wise (Techno) - 2KJa509WSY45vlGHjLL3Q9
    * Knucks (Uk Rap) - 6W4vm8P3JFQboO4cvHeqaa

### Response
This will return the User various information on the artist provided. See below:
```
{
  "external_urls" : {
    "spotify" : "https://open.spotify.com/artist/6W4vm8P3JFQboO4cvHeqaa"
  },
  "followers" : {
    "href" : null,
    "total" : 238032
  },
  "genres" : [ "melodic drill", "uk alternative hip hop" ],
  "href" : "https://api.spotify.com/v1/artists/6W4vm8P3JFQboO4cvHeqaa",
  "id" : "6W4vm8P3JFQboO4cvHeqaa",
  "images" : [ {
    "height" : 640,
    "url" : "https://i.scdn.co/image/ab6761610000e5eb403709b9d33925dabe6681e1",
    "width" : 640
  }, {
    "height" : 320,
    "url" : "https://i.scdn.co/image/ab67616100005174403709b9d33925dabe6681e1",
    "width" : 320
  }, {
    "height" : 160,
    "url" : "https://i.scdn.co/image/ab6761610000f178403709b9d33925dabe6681e1",
    "width" : 160
  } ],
  "name" : "Knucks",
  "popularity" : 68,
  "type" : "artist",
  "uri" : "spotify:artist:6W4vm8P3JFQboO4cvHeqaa"
}
```

### CosmosDB
The ID and Name of the artist gets saved to the CosmosDB to be used again, further development could include disallowing duplicates.
```
{"id":"6W4vm8P3JFQboO4cvHeqaa","name":"Knucks"}
```

To return the database contents to the user, run the following URL in your browser.
```
http://localhost:7055/api/GetDBData
```

This runs the function which retrieves the database data and displays it to the user in the browser.