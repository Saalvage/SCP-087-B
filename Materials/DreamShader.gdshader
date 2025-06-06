shader_type canvas_item;

uniform sampler2D curr;
uniform sampler2D screen : hint_screen_texture;

global uniform float blurStrength;

void fragment() {
	COLOR = vec4(mix(texture(curr, UV).rgb, texture(screen, UV).rgb, blurStrength), 1);
}
