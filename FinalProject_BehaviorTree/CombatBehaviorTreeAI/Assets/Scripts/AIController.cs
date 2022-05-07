using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class AIController : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float lowhealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    private Transform playerTransform;
    private Cover[] availableCovers;

    private Material mat;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;
    private Slider healthBar;

    private Node topNode;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    public bool isBehindCover = false;

    public float CurrentHealth 
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

	// Start is called before the first frame update
	void Start()
    {
        CurrentHealth = startingHealth;
        playerTransform = FindObjectOfType<PlayerController>().transform;
        healthBar = GetComponentInChildren<Slider>();
        availableCovers = FindObjectsOfType<Cover>();
        ConstructbehaviourTree();
    }

	private void Awake()
	{
        mat = GetComponent<MeshRenderer>().material;
        agent = GetComponent<NavMeshAgent>();
    }

	// Update is called once per frame
	void Update()
    {
        topNode.Evaluate();

        if(topNode.NodeState == NodeState.FAILURE)
		{
            SetColor(Color.red);
            agent.isStopped = true;
		}

        healthBar.value = CurrentHealth;

        if(isBehindCover)
		{
            CurrentHealth += Time.deltaTime * healthRestoreRate;
        }

        if(CurrentHealth <= 0)
		{
            Destroy(gameObject);
		}
    }

	public void TakeDamage(float damage)
	{
        CurrentHealth -= damage;
	}

	private void ConstructbehaviourTree()
	{
        // Create cover available node
        IsCoverAvailableNode coverAvaliableNode = new IsCoverAvailableNode(availableCovers, playerTransform, this);

        // Create go to cover node
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);

        // Create health node
        HealthNode healthNode = new HealthNode(this, lowhealthThreshold);

        // Create covered node
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform, this);

        // Create chase node
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);

        // Create chasing range node
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);

        // Create shooting range node
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);

        // Create shoot node
        ShootNode shootNode = new ShootNode(agent, this, playerTransform);

        // Create chase sequence
        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });

        // Create shoot sequence
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        // Create goto cover sequence
        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvaliableNode, goToCoverNode });

        // Create find cover selector
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });

        // Create take cover selector
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });

        // Create main cover selector
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        // set top node
        topNode = new Selector(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });
    }     

    public Transform GetBestCoverSpot()
	{
        return bestCoverSpot;
	}
    public void SetColor(Color color)
	{
        mat.color = color;
	}

    public void SetBestCoverSpot(Transform bestSpot)
    {
        bestCoverSpot = bestSpot;
    }
}
