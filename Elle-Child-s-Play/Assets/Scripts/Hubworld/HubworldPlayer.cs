﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubworldPlayer : MonoBehaviour
{
    public float moveSpeed, rotationSpeed;
    public Transform cam;
    private Rigidbody rb;
    public Fader blackFader;
    public Transform groundT;
    public string chosenScene;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        blackFader.Fade(false, .5f);
    }

    void Update()
    {
        Vector3 upVector = Vector3.up;
        if (Physics.Raycast(groundT.position, Vector3.down, out RaycastHit hit))
            upVector = hit.normal;

        Debug.DrawRay(groundT.position, upVector, Color.red);

        Vector3 lookDir = new Vector3(cam.forward.x, 0, cam.forward.z);
        lookDir = lookDir.normalized;

        Vector3 biTan = Vector3.Cross(upVector, lookDir);

        Debug.DrawRay(groundT.position, biTan, Color.green);

        lookDir = Vector3.Cross(upVector, biTan);

        Debug.DrawRay(groundT.position, lookDir, Color.blue);

        Vector2 s = ELLEAPI.rightHanded ? VRInput.leftStick : VRInput.rightStick;
        float moveAng = Mathf.Atan2(-s.y, s.x) * Mathf.Rad2Deg - 90;
        
        float moveVel = VRInput.leftStick.magnitude * moveSpeed;
        if (moveVel > moveSpeed) moveVel = moveSpeed;
        
        Vector3 moveDir = Quaternion.AngleAxis(moveAng, upVector) * lookDir;

        Debug.DrawRay(groundT.position, moveDir, Color.white);

        rb.velocity = moveDir * moveVel;

        transform.Rotate(Vector3.up * (ELLEAPI.rightHanded ? VRInput.rightStick.x : VRInput.leftStick.x) * rotationSpeed * 10 * Time.deltaTime);

        if ((VRInput.a || VRInput.x) && chosenScene != "")
            StartCoroutine(LoadScene(chosenScene));
    }

    private IEnumerator LoadScene(string scene)
    {
        blackFader.Fade(true, 1f);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(scene);
    }
}