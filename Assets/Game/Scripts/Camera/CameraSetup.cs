using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    [Header("Entity Info")]
    public Transform[] PlayerCharacters;
    private Transform ActivePlayer;
    public Transform[] OpponentCharacters;
    private Transform ActiveOpponent;

    [Header("Camera Setting")]
    public float cameraDistance = 5f;
    public float cameraHeight = 3f;
    public float smoothTime = 0.75f;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        SetActiveCharacters();
    }

    void SetActiveCharacters()
    {
        foreach (var character in PlayerCharacters)
            if (character.gameObject.activeInHierarchy) ActivePlayer = character;

        foreach (var character in OpponentCharacters)
            if (character.gameObject.activeInHierarchy) ActiveOpponent = character;
    }

    private void LateUpdate()
    {
        SetCameraPosition();
    }

    void SetCameraPosition()
    {
        if (ActivePlayer == null || ActiveOpponent == null) return;

        // Calculating mid point between two entities
        Vector3 midPoint = (ActivePlayer.position + ActiveOpponent.position) / 2f;

        // Calculating Direction vector from player to Opponent
        Vector3 PlayerToOpponentDirection = (ActiveOpponent.position - ActivePlayer.position).normalized;

        // Get the Unit PerpendicularDirection vector at 90 deg to the right of the PlayerToOpponentDirection
        // Normalized to find the unit vector
        Vector3 perpendicularDirection = new Vector3(PlayerToOpponentDirection.z, 0, -PlayerToOpponentDirection.x).normalized;

        // Calculate target Camera Position
        Vector3 targetCameraPos = midPoint + perpendicularDirection * cameraDistance;
        targetCameraPos.y = midPoint.y + cameraHeight;

        // Smoothly move the camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetCameraPos, ref velocity, smoothTime);

        // Make the camera look towards a point at certain height above the midpoint
        Vector3 lookAtPoint = new Vector3(midPoint.x, cameraHeight / 2, midPoint.z);
        transform.LookAt(lookAtPoint);
    }


}
