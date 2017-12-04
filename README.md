# 1217-202-P5

## Name: LAB

## Section:

## Description of World:

This project is a simulation of a zen garden named Ariva. The environment is a mixture of Bushido Zen garden and Ancient Rome vineyard, with fairy tales element achieved using particle system.

It will have a simple and meditative design. Its silhouette is a center dot (a tiny pond) inside a 2x bigger solid circle (stone path), inside another 3x bigger dashed circle (sand circle with bonsai trees in between), inside another 4x bigger square (grape vine pergola).

The atmosphere is heavenly and some of the behavior of the creature in the garden does not adhere to their real world counterpart.

## World Exploration:

- A 1st person controller camera will be provided to explore the world
- A camera will follow a flock of butterfly chasing a wandering flower fairy.
- Two camera will be positioned above the overall terrain to inspect the flow field follower ladybugs and the path follower caterpillar
- An UI button will be provided to switch between camera

## Steering Behavior Description

### Flocking:

The butterflies will flock toward a wandering flower fairy that's giving out nectar in a form of a scent trail. They will avoid obstacles which are festival lantern.

### Path Following:

The caterpillar are path following agent who will follow the path defined by the ivy vines. They would be feeding on the vine as they move. The node path are at the foot of the torii gate (the giant archway).

### Flow Field Following:

The ladybugs will follow the flow field. The flow field is generated using the matrix:

```
    x = cos (magnitude(x,y)) * y,
    y = x
```

In 3D plane, replace y with z

### Area(s) of Resistance:

At the food of each torii gate (the giant archway), there is a small puddle which is an area of resistance. The caterpillar is moving through this area and will slow down upon touching the puddle.

## Resources

I experimented with different formula to generate flow field using the web-app <https://anvaka.github.io/fieldplay/> . The final formula is: <https://goo.gl/o87pm5>

Path follower algorithm was created following Nature of Code's guidance, added with some experimentation with wandering. The steering behavior looks similar to what he have for crowd path following.

3D flocking and wandering behavior were the result of my experimentation.

## Notes

- Debug lines are disabled by default, and can be turned on using UI buttons
- LMB means hold the mouse button and drag
- To spawn a random flocker at a random location, press I
- To spawn a random flow field agent at a random location, press O
- To spawn a random path follower at a random location, press P

## Asset Resources:

Four season theme

- Author: Sony Corporation
- Type: Audio, Sound Track
- <https://store.playstation.com/en-us/product/UT0014-PCSA00570_00-CDCTHEMEA0000002>

Water

- Cheryl Fong
- Type: Model
- <https://poly.google.com/view/a0GEnRAf-yd>

Farland Skies - Cloudy Crown/Simply Cumulonimbus

- Author: Borodar
- Type: Texture/Skybox
- <http://u3d.as/rdx> <http://u3d.as/thg>

Standard Assets

- Author: Unity Technologies
- <http://u3d.as/cg6>

Caterpillar

- Type: Model
- Author: Poly by Google
- <https://poly.google.com/view/64T47O5VHAB>

Meditating Cyborg

- Type: Model
- Author: Miguelangelo Rosario
- <https://poly.google.com/view/e29moqKm_hf>

Lady Bug

- Type: Model
- Author: Poly by Google
- <https://poly.google.com/view/0oADEnspgZu>

Butterfly with Animations

- 3D Models/Characters/Animals/Insects
- Gareth Wright
- <http://u3d.as/8YB>

Particle Ribbon

- Type: Particle Systems/Magic
- Author: Moonflower Carnivore
- <http://u3d.as/iqz>

Chinese Lantern Free 3D model

- Type: Model
- Author: xhable
- Author: <https://www.cgtrader.com/free-3d-models/furniture/lamp/chinese-lantern>

Big Sakura Bonsai

- Type: Model
- Author: Michael Grönert
- <https://poly.google.com/view/3tAS6yAx3zN>

Ivy

- Type: Model
- Author: Jarlan Perez
- <https://poly.google.com/view/3SiwWmWJ2WW>

Torii Gate

- Type: Model
- Author: Hattie Stroud
- <https://poly.google.com/view/07__lYTDdEH>

Fantasy Crystal

- Type: 3D model
- Author: vertexdollarstore
- <https://www.cgtrader.com/free-3d-models/character/fantasy/fantasy-crystal>
