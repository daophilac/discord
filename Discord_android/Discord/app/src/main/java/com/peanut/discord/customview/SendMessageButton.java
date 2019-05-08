package com.peanut.discord.customview;

import android.content.Context;
import android.support.v7.widget.AppCompatImageButton;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class SendMessageButton extends AppCompatImageButton {
    public SendMessageButton(Context context) {
        super(context);
    }

    public SendMessageButton(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.sendMessageButtonStyle);
    }

    public SendMessageButton(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
