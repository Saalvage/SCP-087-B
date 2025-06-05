shader_type spatial;
render_mode unshaded;

#include "srgb_to_linear.gdshaderinc"

global uniform vec4 brightness;
global uniform vec2 fog;

// We need this because Godot's default cube's UVs are fucking stupid.
// https://github.com/godotengine/godot-proposals/issues/1395
uniform vec2 uvScale = vec2(1);
uniform float fogScale = 1.0;

uniform sampler2D tex : source_color;

void fragment() {
	ALBEDO = ALBEDO = texture(tex, UV * uvScale).rgb
		* srgb_to_linear(brightness.rgb
			* vec3(1.0 - fogScale * ((length(VERTEX) - fog.x) / (fog.y - fog.x))));
}
