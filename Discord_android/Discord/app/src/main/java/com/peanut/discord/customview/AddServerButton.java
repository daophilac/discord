package com.peanut.discord.customview;

import android.content.Context;
import androidx.appcompat.widget.AppCompatImageButton;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class AddServerButton extends AppCompatImageButton {
    public AddServerButton(Context context) {
        super(context);
    }

    public AddServerButton(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.addServerButtonStyle);
//        setBackground(context.getDrawable(R.drawable.ic_add_circle_black_72dp));
    }

    public AddServerButton(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
