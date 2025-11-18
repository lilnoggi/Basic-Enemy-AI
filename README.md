# Project: Enemy AI - Finite State Machine (FSM)

## Overview
This repository contains the source code for the **Enemy AI** implementation milestone for the module **4001: Basic Enemy AI**.

The primary objective of this project was to develop a **robust, modular, and scalable Enemy AI** using the **Finite State Machine (FSM)** pattern in Unity (C#). The FSM manages the enemy's transition through various behavioral states, primarily focusing on patrol, detection, and pursuit logic.

## Technical Goals & Implemented Features

The AI architecture is structured using the principle of **Composition**, dividing responsibilities across three core scripts:

| Component | Responsibility | Technical Implementation |
| :--- | :--- | :--- |
| **`EnemyStates.cs` (The Brain)** | Manages the **Finite State Machine (FSM)**, controlling all behavioral transitions (e.g., Wander $\to$ DetectedPlayer $\to$ Chase). | Uses a **`switch` statement** on a custom `enum` to execute state-specific logic every frame. |
| **`DistanceToPlayer.cs` (The Sensor)** | Gathers sensory data. Calculates the distance to the player to trigger high-priority state changes. | Implements a **State Priority Check** in `Update()` to prevent conflicts with ongoing Coroutines (e.g., ensuring the AI finishes its 'facing' routine before entering the 'Chase' state). |
| **`Enemy.cs` (The Executor)** | Performs all physical actions. This includes movement, rotation, and delayed sequences. | Utilizes the **NavMeshAgent** for pathfinding (Wander/Chase) and **Coroutines** (`IEnumerator`) for creating controlled, timed delays in behavior. |

---

### **Implemented AI States & Logic**

1.  **Wandering/Patrol State:**
    * Uses **`NavMeshAgent`** to move to randomly generated, validated positions around the enemy's starting point (`startPos`).
    * The **`Wander()`** method uses `Random.insideUnitSphere` and `NavMesh.SamplePosition` to ensure movement is confined to the baked navigation mesh.
2.  **Detection & Chase Sequence:**
    * When the Sensor detects the player (`distanceToPlayer <= detectionDistance`), the AI transitions to `DetectedPlayer`.
    * The **`DetectedPlayer`** state triggers a **Coroutine (`FaceAndChaseRoutine`)** that implements a short, controlled pause (2 seconds), allowing the enemy to complete a **smooth rotation** (`Quaternion.Slerp`) to face the player before fully committing to the chase.
    * The **`Chase`** state uses the **`NavMeshAgent.SetDestination()`** method to continuously update the enemy's target to the player's current position, ensuring pursuit.

## Key Debugging Challenges (FSM Integration)

The most critical challenge encountered was resolving the conflict between the **frame-based input (Sensor)** and **time-based logic (Coroutine)**:

* **The Problem:** The `DistanceToPlayer.Update()` method continuously set the state to `DetectedPlayer` every frame while the player was in range. This repeatedly **canceled the Coroutine**, preventing the transition to the `Chase` state.
* **The Solution:** The Sensor script was modified to include a **State Priority Check**. The Sensor now only triggers a transition to `DetectedPlayer` if the AI is currently in a lower-priority state (`Idle` or `Wandering`). This allowed the FSM's internal Coroutine to manage the progression to the `Chase` state without interruption.

## Setup and Installation

### Requirements
* **Unity:** Version [6000.2 10f1]
* **Unity Packages:** Navigation (NavMesh)

### Steps
1.  Clone this repository to your local machine.
2.  Open the project in the Unity Editor.
3.  Ensure a **NavMesh** has been baked in the scene.
4.  Run the scene containing the Enemy and Player GameObjects.

***

**Project Contributor:** Amani Howe
**Module:** 4001 - Enemy AI (Milestone 3)
