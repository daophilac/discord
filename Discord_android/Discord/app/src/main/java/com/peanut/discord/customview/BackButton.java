package com.peanut.discord.customview;

import android.content.Context;
import androidx.appcompat.widget.AppCompatImageButton;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class BackButton extends AppCompatImageButton {
    public BackButton(Context context) {
        super(context);
    }

    public BackButton(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.backButtonStyle);
    }

    public BackButton(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
