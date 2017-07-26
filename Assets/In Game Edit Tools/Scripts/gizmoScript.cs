using UnityEngine;
using System.Collections;

public class gizmoScript : MonoBehaviour {

	public bool localControl = false;
	[HideInInspector]
	GameObject[] control = null; // Objects which gizmo controls
	public float scaleS = 15; // Scale based on main camera distance
	public bool scales = false;
	[HideInInspector]
	public Vector3[] offset; // Offset from controlled objects
	Vector3 thisOffset = new Vector3 (0, 0, 0); // Offset from gizmo selected obj to mouse
	public GameObject xPlane, yPlane, zPlane, xArrow, yArrow, zArrow; 
	public GameObject xRot, yRot, zRot, looker;
	public GameObject xScale, yScale, zScale, middleScale;
	public GameObject par;
	[HideInInspector]
	public GameObject selected; // Selected gizmo part
	[HideInInspector]
	public Material mat; // Used to assign material when selected
	public Material lastMat; // Used to assign right material to gizmo when deselecting
	public LayerMask maskSel; // This gizmo mask for selection. Dont change it unless you know what you are doing
	Vector3 screenPoint; // Mouse position on screen
	[HideInInspector]
	public static gizmoScript GS;
	public string tagToFind; // With what tag objects to find
	public float gridSnap = 0.25f; // Grid size to snap to
	float gridSnapLast = 0f;
	float gridSnapb = 1;
	[HideInInspector]
	public int type = 0; // 0 for transform, 1 for rotate and 2 for scaling
	public KeyCode moveCode = KeyCode.W;
	public KeyCode rotateCode = KeyCode.E;
	public KeyCode scaleCode = KeyCode.R;
	public KeyCode localControlCode = KeyCode.T;
	Vector2 mos;
	// History variables
	public bool historyEnabled = true;
	public int historySize = 100;
	int currentHistory = 0;
	Vector3[] tempPos, tempScale;
	Quaternion[] tempRot;
	GameObject[] tempID;
	Vector3[,] tempVert;
	Vector3[,] hisPos, hisScale;
	Quaternion[,] hisRot;
	GameObject[,] hisID;
	Vector3[,,] hisVert;
	Vector3 camPos, angl, cam;


	void Awake () {
		camPos = Camera.main.transform.position - transform.position;
		angl = new Vector3(camPos.x / Mathf.Abs (camPos.x), camPos.y / Mathf.Abs (camPos.y), camPos.z / Mathf.Abs (camPos.z));
		par.transform.parent = null;
		GS = this;
		if (historyEnabled) {
			hisPos = new Vector3[historySize, 20]; // 20 is the maximum number of objects that history records
			hisScale = new Vector3[historySize, 20];
			hisRot = new Quaternion[historySize, 20];
			hisID = new GameObject[historySize, 20];
			hisVert = new Vector3[historySize, 20, 5000];
		}
	}

	void addToHistory () {
		if (testIfMoved ()) {
			for (int h = 0; h < tempPos.Length; h++) {
				if (h < 20) { // Here is the same 20, if you change above, don't forget to change this
					hisPos [currentHistory, h] = tempPos [h];
					hisScale [currentHistory, h] = tempScale [h];
					hisRot [currentHistory, h] = tempRot [h];
					hisID [currentHistory, h] = tempID [h];
					for (int ii = 0; ii < 5000; ii++) {
						hisVert [currentHistory, h, ii] = tempVert [h, ii];
					}
				}
			}
			if (currentHistory <= historySize - 1) {
				currentHistory++;
			} else {
				currentHistory = 0;
			}
		}
	}

	bool testIfMoved () {
		bool moved = false;
		if (tempPos != null && tempID[0] == control[0]) {
			for (int ha = 0; ha < control.Length; ha++) {
				if (tempPos [ha] != control [ha].transform.position) {
					moved = true;
					break;
				} else if (tempScale [ha] != control [ha].transform.localScale) {
					moved = true;
					break;
				} else if (tempRot [ha] != control [ha].transform.localRotation) {
					moved = true;
					break;
				}
			}
		}
		return moved;
	}

	void returnHistory () {
		if (currentHistory > 0) {
			currentHistory--;
		} else {
			currentHistory = historySize - 1;
		}
		GameObject hisObj;
		Transform[] child = new Transform[1];
		for (int h = 0; h < 20; h++) {
			if (hisID [currentHistory, h] != null) {
				hisObj = hisID [currentHistory, h];
				if (hisObj.GetComponent<gizmoSelectable> ().ignoreChildTransform) {
					child = new Transform[hisObj.transform.childCount];
					int numb = 0;
					foreach (Transform obj in hisObj.transform) {
						child [numb] = obj;
						numb++;
					}
					foreach (Transform obj in child) {
						obj.parent = null;
					}
				}
				hisObj.transform.position = hisPos [currentHistory, h];
				hisObj.transform.localScale = hisScale [currentHistory, h];
				hisObj.transform.localRotation = hisRot [currentHistory, h];
				if (hisObj.GetComponent<gizmoSelectable> ().ignoreChildTransform) {
					foreach (Transform obj in child) {
						obj.parent = hisObj.transform;
					}
				}
				if (hisObj.GetComponent<MeshFilter>()) {
					Vector3[] ve = hisObj.GetComponent<MeshFilter> ().mesh.vertices;
					for (int ii = 0; ii < ve.Length; ii++) {
						ve [ii] = hisVert [currentHistory, h, ii];
					}
					hisObj.GetComponent<MeshFilter> ().mesh.vertices = ve;
				}
			}
		}
	}

	void saveSelectedState () {
		tempPos = new Vector3[control.Length];
		tempScale = new Vector3[control.Length];
		tempRot = new Quaternion[control.Length];
		tempID = new GameObject[control.Length];
		tempVert = new Vector3[control.Length, 5000];
		for (int h = 0; h < control.Length; h++) {
			tempPos [h] = control [h].transform.position;
			tempScale [h] = control [h].transform.localScale;
			tempRot [h] = control [h].transform.localRotation;
			tempID [h] = control [h];
			Vector3[] ve = control [h].GetComponent<MeshFilter> ().mesh.vertices;
			for (int ii = 0; ii < ve.Length; ii++) { 
				tempVert [h,ii] = ve[ii];
			}
		}
	}

	public void removeControl () {
		control = null;
	}

	public void updateControl () { // Checks if there are any objects selected and assigns them for control
		Vector3 newPos = new Vector3 (0, 0, 0);
		GameObject[] tags = GameObject.FindGameObjectsWithTag (tagToFind);
		int tagsCount = 0;
		GameObject[] tagsObj = new GameObject[tags.Length];
		foreach (GameObject theobj in tags) { // Searches for tags
			if (theobj.GetComponent<gizmoSelectable> ().selected == true) {
				tagsObj [tagsCount] = theobj;
				tagsCount += 1; // Calculates how many tags are found with selected bool true
			}
		}
		if (tagsCount != 0) { // If something is selected assigns to selection
			control = new GameObject[tagsCount];
			for (int i = 0; i < control.Length; i++) {
				control [i] = tagsObj [i];
				if (i == 0) {
					newPos = control [i].transform.position;
				} else {
					newPos += control [i].transform.position;
				}
			}
			newPos = newPos / control.Length; // Moves gizmo to middle of selection
			newPos.x = ((int)(newPos.x*gridSnapb))/gridSnapb; // Snaps to grid
			newPos.y = ((int)(newPos.y*gridSnapb))/gridSnapb;
			newPos.z = ((int)(newPos.z*gridSnapb))/gridSnapb;
			if (!Input.GetMouseButton (0)) {
				transform.position = newPos;
				offset = new Vector3[control.Length];
				for (int i = 0; i < control.Length; i++) { // Finds selected obj offsets
					offset [i] = transform.position - control [i].transform.position;
				}
			}
		}
		if (tagsCount == 0) { // If nothing selected is found
			control = null;
		}
	}

	void reverseSelection() { // Assigns right material and disables selection
		selected.transform.GetComponent<MeshRenderer> ().material = lastMat;
		selected = null;
		thisOffset = Vector3.zero;
	}

	void assignSelection(RaycastHit hitIs) {
		lastMat = hitIs.transform.GetComponent<MeshRenderer> ().material;
		hitIs.transform.GetComponent<MeshRenderer> ().material = mat;
		selected = hitIs.transform.gameObject;
	}

	void Update () {
		cam = Camera.main.transform.position;
		camPos = cam - transform.position;
		if (localControl) {
			par.transform.position = cam;
			Quaternion mDir = transform.rotation;
			par.transform.parent = transform;
			transform.rotation = Quaternion.Euler (Vector3.zero);
			camPos = par.transform.position - transform.position;
			transform.rotation = mDir;
			par.transform.parent = null;
		}
		angl = new Vector3(camPos.x / Mathf.Abs (camPos.x), camPos.y / Mathf.Abs (camPos.y), camPos.z / Mathf.Abs (camPos.z));
		if (gridSnapLast != gridSnap && gridSnap != 0) {
			gridSnapb = 1 / gridSnap;
			gridSnapLast = gridSnap;
		}
		if (gridSnap == 0) {
			gridSnapb = 1f / 0.00001f;
		}
		if (historyEnabled) {
			if (Application.isEditor) { 
				if (Input.GetKeyDown (KeyCode.Z)) {
					returnHistory ();
				}
			} else {
				if (Input.GetKey (KeyCode.LeftControl)) {
					if (Input.GetKeyDown (KeyCode.Z)) {
						returnHistory ();
					}
				}
			}
		}
		if (Input.GetKeyDown (moveCode)) { // Changes to move gizmo
			type = 0;
		}
		if (Input.GetKeyDown (rotateCode)) { // Changes to rotate gizmo
			type = 1;
		}
		if (Input.GetKeyDown (scaleCode)) { // Changes to scale gizmo
			type = 2;
		}
		if (Input.GetKeyDown (localControlCode)) {
			localControl = !localControl;
		}
		Vector3 scale = new Vector3 (1f,1f,1f);
		scale = scale*(Vector3.Distance(Camera.main.transform.position, transform.position)/scaleS); // Scales gizmo according to camera distance 
		transform.localScale = scale;
		updateControl ();
		if (control == null || type != 0) { // Simply hides gizmo if nothing is selected
			xPlane.transform.gameObject.SetActive(false);
			yPlane.transform.gameObject.SetActive(false);
			zPlane.transform.gameObject.SetActive(false);
			xArrow.transform.gameObject.SetActive(false);
			yArrow.transform.gameObject.SetActive(false);
			zArrow.transform.gameObject.SetActive(false);
		} else {
			xArrow.transform.localRotation = Quaternion.Euler (0f, 90f * angl.x, 0f);
			xPlane.transform.localPosition = new Vector3 (0f, 0.25f * angl.y, 0.25f * angl.z);
			zArrow.transform.localRotation = Quaternion.Euler (0f, 90f - 90f * angl.z, 0f);
			zPlane.transform.localPosition = new Vector3 (0.25f * angl.x, 0.25f * angl.y, 0f);
			yArrow.transform.localRotation = Quaternion.Euler (-90f * angl.y, 0f, 0f);
			yPlane.transform.localPosition = new Vector3 (0.25f * angl.x, 0f, 0.25f * angl.z);
			xPlane.transform.gameObject.SetActive(true);
			yPlane.transform.gameObject.SetActive(true);
			zPlane.transform.gameObject.SetActive(true);
			xArrow.transform.gameObject.SetActive(true);
			yArrow.transform.gameObject.SetActive(true);
			zArrow.transform.gameObject.SetActive(true);
			if (localControl) {
				transform.rotation = control [0].transform.localRotation;
			} else {
				transform.rotation = Quaternion.Euler (0, 0, 0);
			}
		}
		if (control == null || type != 1) {
			xRot.transform.gameObject.SetActive(false);
			yRot.transform.gameObject.SetActive(false);
			zRot.transform.gameObject.SetActive(false);
			looker.transform.gameObject.SetActive(false);
		} else {
			xRot.transform.gameObject.SetActive(true);
			yRot.transform.gameObject.SetActive(true);
			zRot.transform.gameObject.SetActive(true);
			looker.transform.gameObject.SetActive(true);
			if (localControl) {
				transform.rotation = control [0].transform.localRotation;
			} else {
				transform.rotation = Quaternion.Euler (0, 0, 0);
			}
		}
		if (control == null || type != 2) {
			xScale.transform.gameObject.SetActive(false);
			yScale.transform.gameObject.SetActive(false);
			zScale.transform.gameObject.SetActive(false);
			middleScale.transform.gameObject.SetActive(false);
		} else {
			xScale.transform.localRotation = Quaternion.Euler (0f, 90f * angl.x, 0f);
			zScale.transform.localRotation = Quaternion.Euler (0f, 90f - 90f * angl.z, 0f);
			yScale.transform.localRotation = Quaternion.Euler (-90f * angl.y, 0f, 0f);
			xScale.transform.gameObject.SetActive(true);
			yScale.transform.gameObject.SetActive(true);
			zScale.transform.gameObject.SetActive(true);
			middleScale.transform.gameObject.SetActive(true);
			if (localControl) {
				transform.rotation = control [0].transform.localRotation;
			} else {
				transform.rotation = Quaternion.Euler (0, 0, 0);
			}
		}
		if (type == 0) { // Moves gizmo
			if (control != null) {
				if (Input.GetMouseButtonUp (0)) {
					foreach (GameObject obj in control) {
						Vector3 pos;
						pos = obj.transform.position;
						pos.x = ((int)(pos.x * gridSnapb)) / gridSnapb;
						pos.y = ((int)(pos.y * gridSnapb)) / gridSnapb;
						pos.z = ((int)(pos.z * gridSnapb)) / gridSnapb;
						obj.transform.position = pos;
						if (historyEnabled) {
							addToHistory ();
						}
					}
				}
			}
			if (Input.GetMouseButtonDown (0)) {
				if (selected != null && historyEnabled) {
					saveSelectedState ();
				}
			}
			if (Input.GetMouseButton (0)) { // If left mouse button is pressed gizmo is controlled and which axis to control is locked
				if (selected != null) {
					screenPoint = Camera.main.WorldToScreenPoint (transform.position);
					Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
					Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint);
					if (thisOffset == Vector3.zero) {
						thisOffset = transform.localPosition - curPosition; // Finds offset from mouse to object transform point, to avoid abjects jumping. It enables smooth transition
					}
					if (selected == xArrow) {
						Vector3 trans = transform.position;
						if (localControl) {
							Vector3 vect = transform.right;
							vect.x = Mathf.Abs (vect.x);
							vect.y = Mathf.Abs (vect.y);
							vect.z = Mathf.Abs (vect.z);
							curPosition += thisOffset;
							curPosition = transform.InverseTransformPoint (curPosition);
							//curPosition.x = 0f;
							curPosition.y = 0f;
							curPosition.z = 0f;
							curPosition = transform.TransformPoint (curPosition);
							curPosition -= thisOffset;
							trans.x = trans.x * (1f - vect.x) + (curPosition.x + thisOffset.x) * vect.x;
							trans.y = trans.y * (1f - vect.y) + (curPosition.y + thisOffset.y) * vect.y;
							trans.z = trans.z * (1f - vect.z) + (curPosition.z + thisOffset.z) * vect.z;
							transform.position = trans;
						} else {
							trans.x = curPosition.x + thisOffset.x;
							transform.position = trans;
						}
					}
					if (selected == yArrow) {
						Vector3 trans = transform.localPosition;
						if (localControl) {
							Vector3 vect = transform.up;
							vect.x = Mathf.Abs (vect.x);
							vect.y = Mathf.Abs (vect.y);
							vect.z = Mathf.Abs (vect.z);
							curPosition += thisOffset;
							curPosition = transform.InverseTransformPoint (curPosition);
							curPosition.x = 0f;
							//curPosition.y = 0f;
							curPosition.z = 0f;
							curPosition = transform.TransformPoint (curPosition);
							curPosition -= thisOffset;
							trans.x = trans.x * (1f - vect.x) + (curPosition.x + thisOffset.x) * vect.x;
							trans.y = trans.y * (1f - vect.y) + (curPosition.y + thisOffset.y) * vect.y;
							trans.z = trans.z * (1f - vect.z) + (curPosition.z + thisOffset.z) * vect.z;
							transform.position = trans;
						} else {
							trans.y = curPosition.y + thisOffset.y;
							transform.position = trans;
						}
					}
					if (selected == zArrow) {
						Vector3 trans = transform.localPosition;
						if (localControl) {
							Vector3 vect = transform.forward;
							vect.x = Mathf.Abs (vect.x);
							vect.y = Mathf.Abs (vect.y);
							vect.z = Mathf.Abs (vect.z);
							curPosition += thisOffset;
							curPosition = transform.InverseTransformPoint (curPosition);
							curPosition.x = 0f;
							curPosition.y = 0f;
							//curPosition.z = 0f;
							curPosition = transform.TransformPoint (curPosition);
							curPosition -= thisOffset;
							trans.x = trans.x * (1f - vect.x) + (curPosition.x + thisOffset.x) * vect.x;
							trans.y = trans.y * (1f - vect.y) + (curPosition.y + thisOffset.y) * vect.y;
							trans.z = trans.z * (1f - vect.z) + (curPosition.z + thisOffset.z) * vect.z;
							transform.position = trans;
						} else {
							trans.z = curPosition.z + thisOffset.z;
							transform.position = trans;
						}
					}
					if (selected == xPlane) {
						Vector3 trans = transform.localPosition;
						if (localControl) {
							Vector3 vect = Vector3.Normalize (transform.forward + transform.up);
							vect.x = Mathf.Abs (vect.x);
							vect.y = Mathf.Abs (vect.y);
							vect.z = Mathf.Abs (vect.z);
							curPosition += thisOffset;
							curPosition = transform.InverseTransformPoint (curPosition);
							curPosition.x = 0f;
							//curPosition.y = 0f;
							//curPosition.z = 0f;
							curPosition = transform.TransformPoint (curPosition);
							curPosition -= thisOffset;
							trans.x = trans.x * (1f - vect.x) + (curPosition.x + thisOffset.x) * vect.x;
							trans.y = trans.y * (1f - vect.y) + (curPosition.y + thisOffset.y) * vect.y;
							trans.z = trans.z * (1f - vect.z) + (curPosition.z + thisOffset.z) * vect.z;
							transform.position = trans;
						} else {
							trans.y = curPosition.y + thisOffset.y;
							trans.z = curPosition.z + thisOffset.z;
							transform.position = trans;
						}
					}
					if (selected == yPlane) {
						Vector3 trans = transform.localPosition;
						if (localControl) {
							Vector3 vect = Vector3.Normalize (transform.forward + transform.right);
							vect.x = Mathf.Abs (vect.x);
							vect.y = Mathf.Abs (vect.y);
							vect.z = Mathf.Abs (vect.z);
							curPosition += thisOffset;
							curPosition = transform.InverseTransformPoint (curPosition);
							//curPosition.x = 0f;
							curPosition.y = 0f;
							//curPosition.z = 0f;
							curPosition = transform.TransformPoint (curPosition);
							curPosition -= thisOffset;
							trans.x = trans.x * (1f - vect.x) + (curPosition.x + thisOffset.x) * vect.x;
							trans.y = trans.y * (1f - vect.y) + (curPosition.y + thisOffset.y) * vect.y;
							trans.z = trans.z * (1f - vect.z) + (curPosition.z + thisOffset.z) * vect.z;
							transform.position = trans;
						} else {
							trans.x = curPosition.x + thisOffset.x;
							trans.z = curPosition.z + thisOffset.z;
							transform.position = trans;
						}
					}
					if (selected == zPlane) {
						Vector3 trans = transform.localPosition;
						if (localControl) {
							Vector3 vect = Vector3.Normalize (transform.right + transform.up);
							vect.x = Mathf.Abs (vect.x);
							vect.y = Mathf.Abs (vect.y);
							vect.z = Mathf.Abs (vect.z);
							curPosition += thisOffset;
							curPosition = transform.InverseTransformPoint (curPosition);
							//curPosition.x = 0f;
							//curPosition.y = 0f;
							curPosition.z = 0f;
							curPosition = transform.TransformPoint (curPosition);
							curPosition -= thisOffset;
							trans.x = trans.x * (1f - vect.x) + (curPosition.x + thisOffset.x) * vect.x;
							trans.y = trans.y * (1f - vect.y) + (curPosition.y + thisOffset.y) * vect.y;
							trans.z = trans.z * (1f - vect.z) + (curPosition.z + thisOffset.z) * vect.z;
							transform.position = trans;
						} else {
							trans.y = curPosition.y + thisOffset.y;
							trans.x = curPosition.x + thisOffset.x;
							transform.position = trans;
						}
					}
					if (control != null) {
						for (int i = 0; i < control.Length; i++) {
							control [i].transform.position = transform.position - offset [i]; // Transforms selected objects based on their offset to gizmo
						}
					}
				}
			} else { // If mouse isn't pressed checks wheather you choose one of the axis to control
				RaycastHit hitIs;
				bool hitedIs = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitIs, Mathf.Infinity, maskSel); // Checks if your mouse is on gizmo, no other objects are taken in to account
				if (hitedIs) {
					if (hitIs.transform.gameObject == xPlane || hitIs.transform.gameObject == yPlane || hitIs.transform.gameObject == zPlane || hitIs.transform.gameObject == xArrow || hitIs.transform.gameObject == yArrow || hitIs.transform.gameObject == zArrow) {
						if (selected == null) {
							assignSelection (hitIs);
						} else {
							reverseSelection ();
							assignSelection (hitIs);
						}
					}
				}
				if (hitedIs != false) {
					if (selected != null && hitIs.transform.gameObject != selected) {
						reverseSelection ();
					}
				} else {
					if (selected != null) {
						reverseSelection ();
					}
				}
			}
		}
		if (type == 1) { // Rotates gizmo
			looker.transform.LookAt(Camera.main.transform.position);
			if (control != null) {
				if (Input.GetMouseButtonUp (0)) { // Snaps to position after rotation
					foreach (GameObject obj in control) {
						Vector3 pos;
						pos = obj.transform.position;
						pos.x = ((int)(pos.x * gridSnapb)) / gridSnapb;
						pos.y = ((int)(pos.y * gridSnapb)) / gridSnapb;
						pos.z = ((int)(pos.z * gridSnapb)) / gridSnapb;
						obj.transform.position = pos;
						if (historyEnabled) {
							addToHistory ();
						}
					}
				}
			}
			if (Input.GetMouseButtonDown (0)) {
				if (selected != null && historyEnabled) {
					saveSelectedState ();
				}
			}
			if (Input.GetMouseButton (0)) { // If left mouse button is pressed gizmo is controlled and which axis to control is locked
				if (selected != null) {
					if (selected == xRot) {
						foreach (GameObject obj in control) {
							if (!localControl) {
								//Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
								obj.transform.RotateAround (transform.position, Vector3.right, Input.mousePosition.y - mos.y);
							} else {
								obj.transform.RotateAround (transform.position, transform.right, Input.mousePosition.y - mos.y);
							}
						}
					}
					if (selected == yRot) {
						foreach (GameObject obj in control) {
							if (!localControl) {
								obj.transform.RotateAround (transform.position, -Vector3.up, Input.mousePosition.x - mos.x);
							} else {
								obj.transform.RotateAround (transform.position, -transform.up, Input.mousePosition.x - mos.x);
							}
						}
					}
					if (selected == zRot) {
						foreach (GameObject obj in control) {
							if (!localControl) {
								obj.transform.RotateAround (transform.position, Vector3.forward, Input.mousePosition.y - mos.y);
							} else {
								obj.transform.RotateAround (transform.position, transform.forward, Input.mousePosition.y - mos.y);
							}
						}
					}
				}
			} else { // If mouse isn't pressed checks wheather you choose one of the axis to control
				RaycastHit hitIs;
				bool hitedIs = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitIs, Mathf.Infinity, maskSel); // Checks if your mouse is on gizmo, no other objects are taken in to account
				if (hitedIs) {
					if (hitIs.transform.gameObject == xRot || hitIs.transform.gameObject == yRot || hitIs.transform.gameObject == zRot) {
						if (selected == null) {
							assignSelection (hitIs);
						} else {
							reverseSelection ();
							assignSelection (hitIs);
						}
					}
				}
				if (hitedIs != false) {
					if (selected != null && hitIs.transform.gameObject != selected) {
						reverseSelection ();
					}
				} else {
					if (selected != null) {
						reverseSelection ();
					}
				}
			}
			mos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y); // Saves mouse last pos for rotation
		}
		if (type == 2) { // Scales gizmo
			if (control != null) {
				if (Input.GetMouseButtonUp (0)) { // Snaps to position after scaling
					foreach (GameObject obj in control) {
						Vector3 pos;
						pos = obj.transform.position;
						pos.x = ((int)(pos.x * gridSnapb)) / gridSnapb;
						pos.y = ((int)(pos.y * gridSnapb)) / gridSnapb;
						pos.z = ((int)(pos.z * gridSnapb)) / gridSnapb;
						obj.transform.position = pos;
						if (historyEnabled) {
							addToHistory ();
						}
					}
				}
			}
			if (Input.GetMouseButtonDown (0)) {
				if (selected != null && historyEnabled) {
					saveSelectedState ();
				}
			}
			if (Input.GetMouseButton (0)) { // If left mouse button is pressed gizmo is controlled and which axis to control is locked
				if (selected != null) {
					Transform temppar;
					Vector3[] verts;
					if (selected == xScale) {
						for (int i = 0; i < offset.Length; i++){
							Vector3 scal = Vector3.MoveTowards (offset [i], offset [i] * 1000, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 20f);
							offset [i].x = scal.x;
							if (scales) {
								if (localControl) {
									control [i].transform.localScale += new Vector3 ((Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f, 0, 0);
								} else {
									if (control [i].GetComponent<MeshFilter>()) {
										temppar = control [i].transform.parent;
										par.transform.position = control [i].transform.position;
										control [i].transform.SetParent (par.transform);
										par.transform.localScale += new Vector3 ((Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f, 0, 0);
										verts = control [i].GetComponent<MeshFilter> ().mesh.vertices;
										for (int ii = 0; ii < verts.Length; ii++) {
											verts[ii] = control [i].transform.TransformPoint (verts[ii]);
										}
										control [i].transform.SetParent (temppar);
										par.transform.localScale = Vector3.one;
										for (int ii = 0; ii < verts.Length; ii++) {
											verts[ii] = control [i].transform.InverseTransformPoint (verts[ii]);
										}
										control [i].GetComponent<MeshFilter> ().mesh.vertices = verts;
									}
								}
							}
						}
					}
					if (selected == yScale) {
						for (int i = 0; i < offset.Length; i++){
							Vector3 scal = Vector3.MoveTowards(offset[i],offset[i]*1000, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x)/20f);
							offset [i].y = scal.y;
							if (scales) {
								if (localControl) {	
									control [i].transform.localScale += new Vector3 (0, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f, 0);
								} else {
									if (control [i].GetComponent<MeshFilter> ()) {
										temppar = control [i].transform.parent;
										par.transform.position = control [i].transform.position;
										control [i].transform.SetParent (par.transform);
										par.transform.localScale += new Vector3 (0, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f, 0);
										verts = control [i].GetComponent<MeshFilter> ().mesh.vertices;
										for (int ii = 0; ii < verts.Length; ii++) {
											verts [ii] = control [i].transform.TransformPoint (verts [ii]);
										}
										control [i].transform.SetParent (temppar);
										par.transform.localScale = Vector3.one;
										for (int ii = 0; ii < verts.Length; ii++) {
											verts [ii] = control [i].transform.InverseTransformPoint (verts [ii]);
										}
										control [i].GetComponent<MeshFilter> ().mesh.vertices = verts;
									}
								}
							}
						}
					}
					if (selected == zScale) {
						for (int i = 0; i < offset.Length; i++){
							Vector3 scal = Vector3.MoveTowards(offset[i],offset[i]*1000, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x)/20f);
							offset [i].z = scal.z;
							if (scales) {
								if (localControl) {
									control [i].transform.localScale += new Vector3 (0, 0, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f);
								} else {
									if (control [i].GetComponent<MeshFilter> ()) {
										temppar = control [i].transform.parent;
										par.transform.position = control [i].transform.position;
										control [i].transform.SetParent (par.transform);
										par.transform.localScale += new Vector3 (0, 0, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f);
										verts = control [i].GetComponent<MeshFilter> ().mesh.vertices;
										for (int ii = 0; ii < verts.Length; ii++) {
											verts [ii] = control [i].transform.TransformPoint (verts [ii]);
										}
										control [i].transform.SetParent (temppar);
										par.transform.localScale = Vector3.one;
										for (int ii = 0; ii < verts.Length; ii++) {
											verts [ii] = control [i].transform.InverseTransformPoint (verts [ii]);
										}
										control [i].GetComponent<MeshFilter> ().mesh.vertices = verts;
									}
								}
							}
						}
					}
					if (selected == middleScale) {
						for (int i = 0; i < offset.Length; i++){
							Vector3 scal = Vector3.MoveTowards(offset[i],offset[i]*1000, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x)/20f);
							offset [i] = scal;
							if (scales) {
								if (localControl) {
									control [i].transform.localScale += new Vector3 ((Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f);
								} else {
									if (control [i].GetComponent<MeshFilter> ()) {
										temppar = control [i].transform.parent;
										par.transform.position = control [i].transform.position;
										control [i].transform.SetParent (par.transform);
										par.transform.localScale += new Vector3 ((Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f, (Input.mousePosition.y - mos.y + Input.mousePosition.x - mos.x) / 50f);
										verts = control [i].GetComponent<MeshFilter> ().mesh.vertices;
										for (int ii = 0; ii < verts.Length; ii++) {
											verts [ii] = control [i].transform.TransformPoint (verts [ii]);
										}
										control [i].transform.SetParent (temppar);
										par.transform.localScale = Vector3.one;
										for (int ii = 0; ii < verts.Length; ii++) {
											verts [ii] = control [i].transform.InverseTransformPoint (verts [ii]);
										}
										control [i].GetComponent<MeshFilter> ().mesh.vertices = verts;
									}
								}
							}
						}
					}
					if (control != null) {
						for (int i = 0; i < control.Length; i++) {
							control [i].transform.position = transform.position - offset [i]; // Transforms selected objects based on their offset to gizmo
						}
					}
				}
			} else { // If mouse isn't pressed checks wheather you choose one of the axis to control
				RaycastHit hitIs;
				bool hitedIs = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitIs, Mathf.Infinity, maskSel); // Checks if your mouse is on gizmo, no other objects are taken in to account
				if (hitedIs) {
					if (hitIs.transform.gameObject == xScale || hitIs.transform.gameObject == yScale || hitIs.transform.gameObject == zScale || hitIs.transform.gameObject == middleScale) {
						if (selected == null) {
							assignSelection (hitIs);
						} else {
							reverseSelection ();
							assignSelection (hitIs);
						}
					}
				}
				if (hitedIs != false) {
					if (selected != null && hitIs.transform.gameObject != selected) {
						reverseSelection ();
					}
				} else {
					if (selected != null) {
						reverseSelection ();
					}
				}
			}
		}
		mos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y); // Saves mouse last pos for scaling
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawLine (transform.position, transform.position + transform.right * 2);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (transform.position, transform.position + transform.forward * 2);
		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, transform.position + transform.up * 2);
	}
}