using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetMsg
{
    string Type();

    void Read(ByteArray buf);

    void Write(ByteArray buf);
}
