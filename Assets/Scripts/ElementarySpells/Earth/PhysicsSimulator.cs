using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsSimulator : MonoBehaviour
{
    CreateSceneParameters parameters;
    Scene predictionScene;
    PhysicsScene predictionPhysicsScene;
    Scene currentScene;
    PhysicsScene currentPhysicsScene;
    public GameObject spellPrefab;
    GameObject duplicate;

    public int accuracy;
    public Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        spellPrefab = gameObject;
        currentVelocity = Vector3.zero;
        parameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene();
        Physics.autoSimulation = false;
        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
        {
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
        duplicate.GetComponent<Rigidbody>().velocity = currentVelocity;
        Simulate();
    }

	public void Init()
	{
        duplicate = Instantiate(spellPrefab);
        SceneManager.MoveGameObjectToScene(duplicate, predictionScene);
        duplicate.transform.position = spellPrefab.transform.position;
	}

	public void Simulate()
    {

        GetComponent<LineRenderer>().positionCount = accuracy;

        for (int i = 0; i < accuracy; i++)
		{
            predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
            GetComponent<LineRenderer>().SetPosition(i, spellPrefab.transform.position);
		}
    }
}
