## Inspiration

We wanted to help the blind and visually impaired navigate the world. 

## What it does

We use computer vision to detect the distance of objects and obstacles around the user. This information is converted to binaural audio, allowing the user to create a mental map of their surroundings.

## How we built it

For our prototype, we use a phone with AR capabilities to capture visual data that can be used to generate a point cloud, which is then stored inside a k-d tree.  Then we can pull out the nearest points to the user, where we place audio sources indicating an object.

## Challenges we ran into

One challenge was connecting all the disparate interfaces we utilised to create our solution. We also had to ensure the accuracy of our depth data. The latency of headphone connections was also something we had to think about.  It was also challenging keeping track of all the different points so that they can be persisted when out of camera view.  We also needed to ensure that only a small number of sounds were playing at once if not the audio engine would become overwhelmed. 

## Accomplishments that we're proud of

We're proud of our demo that really emphasises what a difference our solution can make and how useful audio is in positioning oneself.

## What's next for HearSight

We would love to extend our solution with integration for turn by turn navigation, choices of sounds, using computer vision to identify different objects and play different sounds accordingly, and many more great ideas!