using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeMovement : NetworkBehaviour {

	struct CubeState {
	public int x;
	public int y;
	}
	[SyncVar] CubeState state;
	// Use this for initialization
	void Start ()
	{
		InitState ();
	}

	[Server] void InitState()
	{
		state = new CubeState {	x = (int)transform.position.x,	y = (int)transform.position.y	};
	}

	// Update is called once per frame
	void Update ()
	{
		//if(isLocalPlayer)		//Added so that players do not move all cubes, only their own. Specifically in case of the host.
		{
			KeyCode[] arrowKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow };
			foreach (KeyCode arrowKey in arrowKeys)
			{
				if (!Input.GetKeyDown (arrowKey))
					continue;
				//state = Move(state, arrowKey);
					CmdMoveOnServer(arrowKey);
			}
		}
			SyncState();

	}
	void SyncState ()
	{
		transform.position = new Vector2(state.x, state.y);
	}
	CubeState Move(CubeState previous, KeyCode arrowKey)
	{
		int dx = 0;
		int dy = 0;
		switch (arrowKey)
		{
			case KeyCode.UpArrow:
				dy = 1;
				break;
			case KeyCode.DownArrow:
				dy = -1;
				break;
			case KeyCode.RightArrow:
				dx = 1;
				break;
			case KeyCode.LeftArrow:
				dx = -1;
				break;
		}
		return new CubeState {x = dx + previous.x, y = dy + previous.y};
	}

	[Command]
	void CmdMoveOnServer(KeyCode arrowKey)
	{
		state = Move(state, arrowKey);
	}
}
