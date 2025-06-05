shader_type spatial;
render_mode unshaded;

#include "srgb_to_linear.gdshaderinc"

global uniform vec2 fog;

uniform sampler2D tex : source_color;

void fragment() {
	vec4 sampled = texture(tex, UV);
	ALBEDO = sampled.rgb * vec3(srgb_to_linear(1.0 - ((length(VERTEX) - fog.x) / (fog.y - fog.x))));
	ALPHA = sampled.a;
}
