float srgb_to_linear(float color) {
#if CURRENT_RENDERER == RENDERER_COMPATIBILITY
	return color;
#else
	// Approximation from http://chilliant.blogspot.com/2012/08/srgb-approximations-for-hlsl.html
	color = clamp(color, 0.0, 1.0);
	return color * (color * (color * 0.305306011 + 0.682171111) + 0.012522878);
#endif
}

vec3 srgb_to_linear(vec3 color) {
	return vec3(srgb_to_linear(color.r), srgb_to_linear(color.g), srgb_to_linear(color.b));
}
