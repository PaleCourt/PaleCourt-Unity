using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200070E RID: 1806
public class SpriteFlash : MonoBehaviour
{
	// Token: 0x06001FC4 RID: 8132 RVA: 0x000B17EC File Offset: 0x000AF9EC
	private void Start()
	{
		if (rend == null)
		{
			rend = base.GetComponent<Renderer>();
		}
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000B181C File Offset: 0x000AFA1C
	private void OnDisable()
	{
		if (rend == null)
		{
			rend = base.GetComponent<Renderer>();
		}
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		block.SetFloat("_FlashAmount", 0f);
		rend.SetPropertyBlock(block);
		flashTimer = 0f;
		flashingState = 0;
		repeatFlash = false;
		cancelFlash = false;
		geoFlash = false;
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000B18A4 File Offset: 0x000AFAA4
	private void Update()
	{
		if (cancelFlash)
		{
			block.SetFloat("_FlashAmount", 0f);
			rend.SetPropertyBlock(block);
			flashingState = 0;
			cancelFlash = false;
		}
		if (flashingState == 1)
		{
			if (flashTimer < timeUp)
			{
				flashTimer += Time.deltaTime;
				t = flashTimer / timeUp;
				amountCurrent = Mathf.Lerp(0f, amount, t);
				block.SetFloat("_FlashAmount", amountCurrent);
				rend.SetPropertyBlock(block);
			}
			else
			{
				block.SetFloat("_FlashAmount", amount);
				rend.SetPropertyBlock(block);
				flashTimer = 0f;
				flashingState = 2;
			}
		}
		if (flashingState == 2)
		{
			if (flashTimer < stayTime)
			{
				flashTimer += Time.deltaTime;
			}
			else
			{
				flashTimer = 0f;
				flashingState = 3;
			}
		}
		if (flashingState == 3)
		{
			if (flashTimer < timeDown)
			{
				flashTimer += Time.deltaTime;
				t = flashTimer / timeDown;
				amountCurrent = Mathf.Lerp(amount, 0f, t);
				block.SetFloat("_FlashAmount", amountCurrent);
				rend.SetPropertyBlock(block);
			}
			else
			{
				block.SetFloat("_FlashAmount", 0f);
				rend.SetPropertyBlock(block);
				flashTimer = 0f;
				if (repeatFlash)
				{
					flashingState = 1;
				}
				else
				{
					flashingState = 0;
				}
			}
		}
		if (geoFlash)
		{
			if (geoTimer > 0f)
			{
				geoTimer -= Time.deltaTime;
			}
			else
			{
				FlashingSuperDash();
				geoFlash = false;
			}
		}
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000B1AEC File Offset: 0x000AFCEC
	public void GeoFlash()
	{
		geoFlash = true;
		geoTimer = 0.25f;
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000B1B00 File Offset: 0x000AFD00
	public void flash(Color flashColour_var, float amount_var, float timeUp_var, float stayTime_var, float timeDown_var)
	{
		flashColour = flashColour_var;
		amount = amount_var;
		timeUp = timeUp_var;
		stayTime = stayTime_var;
		timeDown = timeDown_var;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000B1B6C File Offset: 0x000AFD6C
	public void CancelFlash()
	{
		cancelFlash = true;
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x000B1B78 File Offset: 0x000AFD78
	public void FlashingSuperDash()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.7f;
		timeUp = 0.1f;
		stayTime = 0.01f;
		timeDown = 0.1f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = true;
		SendToChildren(new Action(FlashingSuperDash));
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x000B1C18 File Offset: 0x000AFE18
	public void FlashingGhostWounded()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.7f;
		timeUp = 0.5f;
		stayTime = 0.01f;
		timeDown = 0.5f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = true;
		SendToChildren(new Action(FlashingGhostWounded));
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x000B1CB8 File Offset: 0x000AFEB8
	public void FlashingWhiteStay()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.6f;
		timeUp = 0.01f;
		stayTime = 999f;
		timeDown = 0.01f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = true;
		SendToChildren(new Action(FlashingWhiteStay));
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000B1D58 File Offset: 0x000AFF58
	public void FlashingWhiteStayMoth()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.6f;
		timeUp = 2f;
		stayTime = 9999f;
		timeDown = 2f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = true;
		SendToChildren(new Action(FlashingWhiteStayMoth));
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000B1DF8 File Offset: 0x000AFFF8
	public void FlashingFury()
	{
		Start();
		flashColour = new Color(0.71f, 0.18f, 0.18f);
		amount = 0.75f;
		timeUp = 0.25f;
		stayTime = 0.01f;
		timeDown = 0.25f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = true;
		SendToChildren(new Action(FlashingFury));
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x000B1EA0 File Offset: 0x000B00A0
	public void FlashingOrange()
	{
		flashColour = new Color(1f, 0.31f, 0f);
		amount = 0.7f;
		timeUp = 0.1f;
		stayTime = 0.01f;
		timeDown = 0.1f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = true;
		SendToChildren(new Action(FlashingOrange));
	}

	// Token: 0x06001FD0 RID: 8144 RVA: 0x000B1F40 File Offset: 0x000B0140
	public void flashInfected()
	{
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		flashColour = new Color(1f, 0.31f, 0f);
		amount = 0.9f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.25f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashInfected));
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x000B1FF4 File Offset: 0x000B01F4
	public void flashDung()
	{
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		flashColour = new Color(0.45f, 0.27f, 0f);
		amount = 0.9f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.25f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashDung));
	}

	// Token: 0x06001FD2 RID: 8146 RVA: 0x000B20A8 File Offset: 0x000B02A8
	public void flashDungQuick()
	{
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		flashColour = new Color(0.45f, 0.27f, 0f);
		amount = 0.75f;
		timeUp = 0.001f;
		stayTime = 0.05f;
		timeDown = 0.1f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashDungQuick));
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000B215C File Offset: 0x000B035C
	public void flashSporeQuick()
	{
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		flashColour = new Color(0.95f, 0.9f, 0.15f);
		amount = 0.75f;
		timeUp = 0.001f;
		stayTime = 0.05f;
		timeDown = 0.1f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashSporeQuick));
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000B2210 File Offset: 0x000B0410
	public void flashWhiteQuick()
	{
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		flashColour = new Color(1f, 1f, 1f);
		amount = 1f;
		timeUp = 0.001f;
		stayTime = 0.05f;
		timeDown = 0.001f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashWhiteQuick));
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x000B22C4 File Offset: 0x000B04C4
	public void flashInfectedLong()
	{
		flashColour = new Color(1f, 0.31f, 0f);
		amount = 0.9f;
		timeUp = 0.01f;
		stayTime = 0.25f;
		timeDown = 0.35f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashInfectedLong));
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x000B2364 File Offset: 0x000B0564
	public void flashArmoured()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.9f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.25f;
		if (block != null)
		{
			block.Clear();
			block.SetColor("_FlashColor", flashColour);
		}
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashArmoured));
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x000B240C File Offset: 0x000B060C
	public void flashBenchRest()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.7f;
		timeUp = 0.01f;
		stayTime = 0.1f;
		timeDown = 0.75f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashBenchRest));
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x000B24AC File Offset: 0x000B06AC
	public void flashDreamImpact()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.9f;
		timeUp = 0.01f;
		stayTime = 0.25f;
		timeDown = 0.75f;
		if (block != null)
		{
			block.Clear();
			block.SetColor("_FlashColor", flashColour);
		}
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashDreamImpact));
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x000B2554 File Offset: 0x000B0754
	public void flashMothDepart()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.75f;
		timeUp = 1.9f;
		stayTime = 1f;
		timeDown = 0.25f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashMothDepart));
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x000B25F4 File Offset: 0x000B07F4
	public void flashSoulGet()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.5f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.25f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashSoulGet));
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x000B2694 File Offset: 0x000B0894
	public void flashShadeGet()
	{
		flashColour = new Color(0f, 0f, 0f);
		amount = 0.5f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.25f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashShadeGet));
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x000B2734 File Offset: 0x000B0934
	public void flashWhiteLong()
	{
		flashColour = new Color(1f, 1f, 1f);
		amount = 1f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 2f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashWhiteLong));
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x000B27D4 File Offset: 0x000B09D4
	public void flashOvercharmed()
	{
		flashColour = new Color(0.72f, 0.376f, 0.72f);
		amount = 0.75f;
		timeUp = 0.2f;
		stayTime = 0.01f;
		timeDown = 0.5f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashOvercharmed));
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000B2874 File Offset: 0x000B0A74
	public void flashFocusHeal()
	{
		Start();
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.85f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.35f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashFocusHeal));
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x000B291C File Offset: 0x000B0B1C
	public void flashFocusGet()
	{
		Start();
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.5f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.35f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashFocusGet));
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000B29C4 File Offset: 0x000B0BC4
	public void flashWhitePulse()
	{
		Start();
		flashColour = new Color(1f, 1f, 1f);
		amount = 0.35f;
		timeUp = 0.5f;
		stayTime = 0.01f;
		timeDown = 0.75f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashWhitePulse));
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000B2A6C File Offset: 0x000B0C6C
	public void flashHealBlue()
	{
		flashColour = new Color(0f, 0.584f, 1f);
		amount = 0.75f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.5f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(flashHealBlue));
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000B2B0C File Offset: 0x000B0D0C
	public void FlashShadowRecharge()
	{
		Start();
		flashColour = new Color(0f, 0f, 0f);
		amount = 0.75f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.35f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(FlashShadowRecharge));
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x000B2BB4 File Offset: 0x000B0DB4
	public void flashInfectedLoop()
	{
		flashColour = new Color(1f, 0.31f, 0f);
		amount = 0.9f;
		timeUp = 0.2f;
		stayTime = 0.01f;
		timeDown = 0.2f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = true;
		SendToChildren(new Action(flashInfectedLoop));
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000B2C54 File Offset: 0x000B0E54
	public void FlashGrimmflame()
	{
		Start();
		flashColour = new Color(1f, 0.25f, 0.25f);
		amount = 0.75f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 1f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(FlashGrimmflame));
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000B2CFC File Offset: 0x000B0EFC
	public void FlashGrimmHit()
	{
		Start();
		flashColour = new Color(1f, 0.25f, 0.25f);
		amount = 0.75f;
		timeUp = 0.01f;
		stayTime = 0.01f;
		timeDown = 0.25f;
		block.Clear();
		block.SetColor("_FlashColor", flashColour);
		flashingState = 1;
		flashTimer = 0f;
		repeatFlash = false;
		SendToChildren(new Action(FlashGrimmHit));
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000B2DA4 File Offset: 0x000B0FA4
	private void SendToChildren(Action function)
	{
		if (!sendToChildren)
		{
			return;
		}
		SpriteFlash[] componentsInChildren = base.GetComponentsInChildren<SpriteFlash>();
		foreach (SpriteFlash spriteFlash in componentsInChildren)
		{
			if (!(spriteFlash == this))
			{
				spriteFlash.sendToChildren = false;
				Type type = spriteFlash.GetType();
				MethodInfo method = type.GetMethod(function.Method.Name);
				method.Invoke(spriteFlash, null);
			}
		}
	}

	// Token: 0x0400214C RID: 8524
	private Renderer rend;

	// Token: 0x0400214D RID: 8525
	private Color flashColour;

	// Token: 0x0400214E RID: 8526
	private float amount;

	// Token: 0x0400214F RID: 8527
	private float timeUp;

	// Token: 0x04002150 RID: 8528
	private float stayTime;

	// Token: 0x04002151 RID: 8529
	private float timeDown;

	// Token: 0x04002152 RID: 8530
	private int flashingState;

	// Token: 0x04002153 RID: 8531
	private float flashTimer;

	// Token: 0x04002154 RID: 8532
	private float amountCurrent;

	// Token: 0x04002155 RID: 8533
	private float t;

	// Token: 0x04002156 RID: 8534
	private bool repeatFlash;

	// Token: 0x04002157 RID: 8535
	private bool cancelFlash;

	// Token: 0x04002158 RID: 8536
	private float geoTimer;

	// Token: 0x04002159 RID: 8537
	private bool geoFlash;

	// Token: 0x0400215A RID: 8538
	private MaterialPropertyBlock block;

	// Token: 0x0400215B RID: 8539
	private bool sendToChildren = true;
}
