using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMissiles : AbstractSpell
{
    public WaterBall BallPrefab;
    
    public float TimePerBall = 0.12f;

    private float timeLocale = 0f;
    [HideInInspector]
    public Transform targetTransform;

    public float maxSpellTime;

    /// <summary>
    /// New fov value during aiming
    /// </summary>
    [SerializeField] [Min(0)] private float zoomPower;

    private List<WaterBall> balls = new List<WaterBall>(20);
    private int spawnedballs = 0;

    private GameModeSingleton gameManager;

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        elementary.GetComponent<MeshRenderer>().enabled = false;
        gameManager = GameModeSingleton.GetInstance();
        gameManager.GetCinemachineCameraController.ZoomIn();

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Time locale
        timeLocale += Time.fixedDeltaTime;
        // Ball spawning
        if (!isReleased() && spawnedballs * TimePerBall < timeLocale) {
            spawnedballs++;
            WaterBall ball = Instantiate(BallPrefab, transform.position, Quaternion.identity);
            ball.parent = this;
            balls.Add(ball);
            ball.GetComponent<WaterBall>().targetLocation = getDestination();
        }
        // Target location update
        balls.ForEach(e => { e.targetLocation = getDestination(); });
        // Spell self destruction
        float castmax = maxSpellTime < 0.1f ? 1 : maxSpellTime;
        if (timeLocale > castmax || balls.Count <= 0)
        {
            Terminate();
        }

    }

    /// <returns>The destination position of the child missiles. </returns>
    private Vector3 getDestination()
    {
        if (targetTransform == null) return base.target;
        return targetTransform.position;
    }

    protected override void onChargeEnd(float chargetime)
    {
        base.onChargeEnd(chargetime);
        gameManager.GetCinemachineCameraController.ZoomOut();
        balls.ForEach(e => { e.launched = true; });
    }

    public void RemoveBall(WaterBall ball) {
        balls.Remove(ball);
        Destroy(ball.gameObject);
    }

    public override void Terminate()
    {
        balls.ForEach(e => { Destroy(e.gameObject); });
        elementary.GetComponent<ElementaryController>().currentSpell = null;
        elementary.transform.position = getDestination();
        elementary.GetComponent<ElementaryController>().computePosition = true;
        elementary.GetComponent<MeshRenderer>().enabled = true;
        Destroy(gameObject);
    }
}
