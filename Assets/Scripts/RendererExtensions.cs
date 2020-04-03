using UnityEngine;
 
 // This is a simple extension for Renderer components to check whether a renderer is
 // visible by a given camera. 
public static class RendererExtensions {
	public static bool IsVisibleFrom(this Renderer renderer, Camera camera) {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
}