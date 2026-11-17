# Week 6 – Unity AI Navigation Project

## Project Overview

This project was developed as part of the Week 6 assignment on **Navigation Meshes (NavMesh)** in Unity.

The main objective is to explore Unity's AI Navigation system and understand how an AI agent navigates through an environment containing different terrain areas, navigation links, and dynamic obstacles.

The project focuses on NavMesh generation, area-based movement costs, route selection, Off-Mesh Link traversal, and dynamic path recalculation.

## Technologies and Tools

- Unity
- Unity AI Navigation
- NavMeshSurface
- NavMeshModifier
- NavMeshModifierVolume
- NavMeshObstacle
- NavMeshLink
- NavMeshAgent
- Unity Input System
- C#

## Scene and Environment Setup

A custom navigation environment was created using a Plane as the ground surface and several wall objects as obstacles.

The scene contains:

- A walkable ground surface generated with NavMeshSurface.
- A U-shaped wall structure with non-walkable boundaries.
- A Mud area defined using NavMeshModifierVolume.
- A Water area defined using NavMeshModifierVolume.
- A NavMeshLink connecting navigation points across a wall.
- A sliding door controlled during runtime.
- A moving obstacle with NavMeshObstacle and Carve enabled.
- An NPC agent and a target object for pathfinding experiments.

The NavMeshSurface component collects the selected scene geometry and generates the navigation mesh. NavMeshModifier is used to prevent the NPC from walking through the walls.

The Mud and Water areas are assigned separate navigation area types so that their traversal costs can be configured independently.

## Implemented Features

### 1. NavMesh Generation

NavMeshSurface was added to a navigation manager object and configured to collect the scene geometry using physics colliders.

The ground and obstacle layers were included in the surface settings. After baking the NavMesh, walkable areas and non-walkable wall boundaries could be observed in the Unity Scene view.

### 2. Custom Navigation Areas

Two custom navigation areas were created:

- **Mud (Camur):** A higher-cost terrain area used to investigate how traversal costs influence route selection.
- **Water (Su):** A separate navigation area with its own area type.

NavMeshModifierVolume components were used to define the boundaries of both areas.

### 3. Jump Link Navigation

A NavMeshLink was created to connect navigation points across a wall.

A custom `JumpLinkTraverser` script was implemented to control the NPC's movement across the Off-Mesh Link using a curved jump trajectory.

The link can be activated or deactivated during runtime, allowing the effect of link availability and traversal cost on route selection to be tested.

### 4. Sliding Door

A sliding door was implemented using a NavMeshObstacle component with Carve enabled.

The `SlidingDoor` script moves the door between its open and closed positions. The obstacle component is enabled or disabled according to the door state so that the NPC can respond to the changed navigation conditions.

### 5. Moving Obstacle

A moving obstacle was created using a cube, a NavMeshObstacle component, and a custom movement script.

The obstacle moves continuously between two positions. Carve is enabled, allowing the navigation system to respond to the changing obstacle position.

### 6. Path Visualization and Cost Display

A LineRenderer is used to visualize the NPC's calculated path.

The `PathDebugDraw` script displays the following runtime information:

- Path length
- Estimated total path cost
- Number of path corners
- Path status

The `AreaCostToggle` script allows the Mud area cost and Jump Link state to be changed during runtime.

## Custom C# Scripts

| Script | Description |
|---|---|
| `NpcNavChase.cs` | Updates the NPC destination and visualizes its current path. |
| `JumpLinkTraverser.cs` | Controls the NPC's curved jump across an Off-Mesh Link. |
| `AreaCostToggle.cs` | Changes the Mud cost and toggles the Jump Link. |
| `PathDebugDraw.cs` | Displays path length, estimated cost, corner count, and path status. |
| `SlidingDoor.cs` | Controls the sliding door movement and obstacle state. |
| `MovingObstacle.cs` | Moves the obstacle between two positions. |

## Runtime Controls

The following keyboard controls are used during the experiments:

| Key | Function |
|---|---|
| M | Changes the Mud area cost. |
| J | Activates or deactivates the Jump Link. |
| K | Opens or closes the sliding door. |

The Mud area cost cycles through these values:

`1 → 2 → 5 → 10`

## Experiments and Results

Several experiments were conducted to observe how the NavMeshAgent responds to different area costs, link settings, and environmental changes.

### Experiment 1 – Mud Area Cost

The NPC and target were temporarily positioned on opposite sides of the Mud area. The Mud cost was changed during runtime.

| Mud Cost | Approx. Path Length | Approx. Total Cost |
|---|---:|---:|
| 1 | 35 | 37 |
| 2 | 36 | 39 |
| 5 | 36 | 39 |

**Observation:**

Increasing the Mud cost increased the estimated total path cost. However, the path length changed only slightly, and a clear route switch was not confirmed in these tests.

These values are approximate readings from the runtime debug display.

### Experiment 2 – Jump Link Cost

The NPC and target were positioned on opposite sides of a wall to test the NavMeshLink.

Two main conditions were observed:

- With the link activated and Cost Override disabled, the NPC used the jump connection.
- With Cost Override enabled and set to 5, the NPC avoided the jump connection and selected a longer alternative route.

**Observation:**

The experiment demonstrated that the cost assigned to a navigation link can influence the route selected by the NavMeshAgent.

### Experiment 3 – Sliding Door

The sliding door was tested in both open and closed states.

- When the door was open, the NPC followed the available route.
- When the door was closed, the carved obstacle blocked the previous passage.
- The NPC recalculated its path and selected an alternative route.

The displayed total path cost increased by approximately 1 in the observed test.

### Experiment 4 – Moving Obstacle

The moving obstacle was tested with Carve enabled.

The obstacle moved continuously, and the NPC avoided it while navigating toward the target.

**Observation:**

The experiment demonstrated that the navigation system could respond to a moving carved obstacle during runtime.

## Path Cost Analysis

The experiments demonstrated that route selection depends on both route geometry and traversal costs.

The Mud tests showed a slight increase in estimated path cost as the Mud cost increased. However, a definite route change was not confirmed in those measurements.

The Jump Link experiment provided a clear route change: the NPC used the link with Cost Override disabled but selected the alternative route when the override was set to 5.

The exact numerical threshold for this route change requires additional controlled measurements of the jump route cost and the alternative route cost.

## Conclusion

This project provided practical experience with Unity AI Navigation and NavMesh-based pathfinding.

By creating custom navigation areas, implementing a jump connection, and introducing dynamic obstacles, I explored how different environmental conditions influence an AI agent's movement and route selection.

The experiments demonstrated that navigation costs can affect route decisions, NavMeshLink can provide an alternative connection across obstacles, and carved obstacles can cause the agent to recalculate its path.

Overall, the project improved my understanding of NavMesh generation, navigation area configuration, path cost evaluation, Off-Mesh Link traversal, and dynamic obstacle handling in Unity.

## Project Status

The main navigation environment and interactive features have been implemented and tested.

The recorded experiment values are preliminary observations. Exact manual route-cost calculations, a numerical route-change threshold, and the final comparison screenshots are still subject to completion.

The project report and supporting screenshots are intended to be stored in the `Report` folder.