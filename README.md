A first-person exploration game set on **Titan**, Saturn's moon, built in Unity 6 as a final project for Computer Graphics.

## About the Game

The player crash-lands on Titan and must navigate a hostile environment with a damaged suit. As oxygen drains and the storm closes in, they must find repair stations, avoid toxic plants, and make their way to safety.

## Features

- **Damaged suit system** — player starts with limited movement (no jump height, no dash, reduced speed) until they find the repair station
- **Oxygen & suit integrity** — oxygen drains over time; suit takes damage from toxic plants and storm zones; if both hit zero, the player dies and respawns
- **Fog storm corridor** — a dynamic fog system that thickens and deals damage when the player strays from the safe path
- **Oxygen vents** — hold E to refill oxygen at designated vent stations
- **Checkpoints** — refill all stats at checkpoint stations along the route
- **Toxic plants** — damage the suit on contact
- **Full movement system** — includes coyote time, jump buffering, double jump, dash, and inertia

## Project Structure

```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerMovement.cs      # Movement, jump, dash, damaged state
│   │   ├── PlayerStats.cs         # Oxygen and suit integrity management
│   │   └── MouseLook.cs           # Camera mouse look
│   ├── Environment/
│   │   ├── RepairStation.cs       # Hold E to repair suit and unlock abilities
│   │   ├── Checkpoints.cs         # Refill all stats on contact
│   │   ├── OxygenVent.cs          # Hold E to refill oxygen
│   │   ├── DamagePlant.cs         # Damages suit on contact
│   │   ├── FogZone.cs             # Manages global fog and storm damage
│   │   ├── FogZoneTrigger.cs      # Trigger volumes for safe corridor zones
│   │   └── EndLvl.cs              # Triggers end of level cinematic
│   └── Editor/
│       └── AddMeshColliders.cs    # Utility: adds mesh colliders to all prefabs
├── Scenes/
│   ├── intro.unity
│   └── level 1.unity
└── Prefabs/
    ├── mountains.prefab
    ├── rocks.prefab
    ├── trees.prefab
    ├── geisers.prefab
    └── damage.prefab
```

## Controls

| Action | Key |
|--------|-----|
| Move | WASD |
| Jump | Space |
| Dash (repaired only) | Left Shift |
| Interact / Refill | E (hold) |
| Look | Mouse |
