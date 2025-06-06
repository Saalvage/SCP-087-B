shader_type canvas_item;

uniform sampler2D tex;

global uniform float blurStrength;

// I really don't know why this shader is necessary.
// Just using the dreamfilter viewport texture on the sprite results in nothing rendering..
void fragment() {
	COLOR = vec4(texture(tex, UV).rgb, blurStrength);
}
