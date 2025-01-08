using System.Collections;

public interface IAttackable
{
    void TakeDamage(int damage);
    IEnumerator DamagedColorChange();
}
