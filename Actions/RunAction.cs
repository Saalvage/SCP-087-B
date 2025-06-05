public partial class RunAction : LightsAction {
    protected override void AddEnemy() {
        base.AddEnemy();
        _enemy!.Speed = 2.1f;
    }

    protected override void RemoveEnemy() {
        base.RemoveEnemy();
        Helpers.SetBrightness(Player.Brightness);
        Player.ResetFogRange();
    }

    private void EnableRun() {
        _enemy!.Speed = 1.8f;
        Helpers.SetBrightness(15);
        Player.ResetFogRange();
    }

    private void DisableRun() {
        AudioManager.PlaySound(Resources.HorrorSounds[0]);
        _enemy!.Speed = 0f;
        Helpers.SetBrightness(Player.Brightness);
        Player.SetFogRange(Player.FogNear, 20);
    }
}
