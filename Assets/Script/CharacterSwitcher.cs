using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcherNewInput : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    private int activeCharacterIndex = 0;
	private CameraFollow cameraFollow;
	private bool[] unlocked;

    void Start()
    {
		cameraFollow = Camera.main.GetComponent<CameraFollow>();
		
		// first character starts unlocked, others locked
        unlocked = new bool[characters.Length];
        unlocked[0] = true;

		// Assign switcher reference to players
        foreach (var c in characters)
            c.GetComponent<PlayerMovement>().switcher = this;

        // Disable all characters except the first one
        for (int i = 1; i < characters.Length; i++)
        {
            characters[i].SetActive(false); // invisible
        }

        SetActiveCharacter(0);
    }

	public void SwitchToNextCharacter()
    {
        int next = activeCharacterIndex + 1;

        if (next < characters.Length)
        {
            unlocked[next] = true;
            characters[next].SetActive(true);   // now becomes visible
            SetActiveCharacter(next);
        }
    }

	public void SetActiveCharacter(int index)
	{
		if (!unlocked[index])
			return; // can't switch to locked characters

		activeCharacterIndex = index;

		for (int i = 0; i < characters.Length; i++)
		{
			bool active = i == index;

			PlayerMovement mv = characters[i].GetComponent<PlayerMovement>();
			Rigidbody rb = characters[i].GetComponent<Rigidbody>();
			Collider col = characters[i].GetComponent<Collider>();

			if (mv != null) mv.enabled = active;

			if (rb != null)
			{
				rb.isKinematic = !active;

				// Only reset velocity if Rigidbody is NOT kinematic
				if (!rb.isKinematic)
				{
					rb.linearVelocity = Vector3.zero;
					rb.angularVelocity = Vector3.zero;
				}
			}

			if (col != null) col.enabled = active;
		}

		// Update camera target
		cameraFollow.SetTarget(characters[index].transform);

		// Update pivot if assigned
		var movement = characters[index].GetComponent<PlayerMovement>();
		if (movement != null && movement.cameraPivot != null)
			cameraFollow.SetPivot(movement.cameraPivot);
	}

	// Getter for the active character
    public GameObject GetActiveCharacter()
    {
        return characters[activeCharacterIndex];
    }
}
